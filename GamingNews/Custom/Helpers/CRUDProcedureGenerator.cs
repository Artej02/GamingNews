using Microsoft.Extensions.Configuration;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Helpers
{
    public class CRUDProcedureGenerator
    {
        private string DbName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CrudHelper")["DbName"];
        private string Schema = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CrudHelper")["Schema"];


        public async Task<CRUDResponse> GenerateProcedure(string TableName)
        {
            CRUDResponse response = new CRUDResponse();
            var ProcedureName = $"CreateUpdateDelete{TableName}";

            #region Check if procedure exist
            // Check if procedure exists return response
            var exists = await new Query().SelectSingle<int>($"SELECT isnull(1,0) FROM sys.procedures WHERE Name = '{ProcedureName}'");
            if (exists.Result != 0)
            {
                response.Code = 1;
                response.HasError = exists.HasError;
                response.Message = exists.HasError ? exists.ErrorMessage : $"Stored procedure with the name {ProcedureName} exists!";
                return response;

            }
            #endregion

            #region Check if table exists
            //Check if table Exists return response
            var queryTableColumns = $"SELECT I.COLUMN_NAME, I.DATA_TYPE, I.CHARACTER_MAXIMUM_LENGTH  FROM INFORMATION_SCHEMA.COLUMNS I WHERE TABLE_SCHEMA = '{Schema}' AND TABLE_NAME = '{TableName}'";
            var TableColumns = await new Query().Select<SqlColumnProperties>(queryTableColumns);
            if (TableColumns.Result.Count() == 0)
            {
                response.Code = 0;
                response.HasError = TableColumns.HasError;
                response.Message = TableColumns.HasError ? TableColumns.ErrorMessage : "Table does not exist!";
                return response;
            }
            #endregion

            #region Start creating procedure
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Create PROCEDURE [{Schema}].[{ProcedureName}]")
            .AppendLine(" @CRUDOperation         TINYINT");
            foreach (var column in TableColumns.Result)
            {
                var name = column.COLUMN_NAME;
                var type = column.CHARACTER_MAXIMUM_LENGTH == null ? column.DATA_TYPE : column.CHARACTER_MAXIMUM_LENGTH < 0 ? $"{column.DATA_TYPE}(MAX)" : $"{column.DATA_TYPE}({column.CHARACTER_MAXIMUM_LENGTH})";

                sb.AppendLine($",@{name}     {type} = NULL ");
            }
            sb.AppendLine("AS").AppendLine("BEGIN");
            sb.AppendLine("IF @CRUDOperation = 1").AppendLine("BEGIN");
            sb.AppendLine($"INSERT INTO [{Schema}].[{TableName}]").AppendLine("(");

            int counter = 0;
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            StringBuilder updateColumns = new StringBuilder();
            foreach (var column in TableColumns.Result)
            {

                if (counter == 0)
                {
                    counter++;
                }
                else if (counter == 1)
                {

                    columns.AppendLine($" [{column.COLUMN_NAME}]");
                    values.AppendLine($"@{column.COLUMN_NAME}");
                    updateColumns.AppendLine($" [{column.COLUMN_NAME}] = @{column.COLUMN_NAME}");
                    counter++;
                }
                else
                {
                    columns.AppendLine($",[{column.COLUMN_NAME}]");
                    values.AppendLine($",@{column.COLUMN_NAME}");
                    updateColumns.AppendLine($",[{column.COLUMN_NAME}] = @{column.COLUMN_NAME}");
                }

            }
            sb.AppendLine(columns.ToString()).AppendLine(")");
            sb.AppendLine("OUTPUT inserted.Id");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine(values.ToString()).AppendLine(")").AppendLine("END");
            sb.AppendLine("IF @CRUDOperation = 2");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"UPDATE [{Schema}].[{TableName}]");
            sb.AppendLine("SET");
            sb.AppendLine(updateColumns.ToString());
            sb.AppendLine("WHERE Id = @Id");
            sb.AppendLine("END");
            sb.AppendLine("IF @CRUDOperation = 3");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"DELETE FROM [{Schema}].[{TableName}]");
            sb.AppendLine("WHERE Id = @Id");
            sb.AppendLine("END");
            sb.AppendLine("END");

            var executeCRUD = sb.ToString();

            var createQuery = await new Query().Execute(executeCRUD);

            if (createQuery.HasError)
            {
                response.Code = 0;
                response.HasError = createQuery.HasError;
                response.Message = createQuery.ErrorMessage;
                return response;
            }


            response.Code = 1;
            response.HasError = createQuery.HasError;
            response.Message = $"Successfully created procedure {ProcedureName}";

            //var modelResponse = await GenerateModel(TableColumns.Result.ToList(), TableName);
            //response.Model = modelResponse;
            #endregion

            return response;

        }

        private async Task<CRUDResponse> GenerateModel(List<SqlColumnProperties> column, string ModelName)
        {
            CRUDResponse response = new CRUDResponse();

            var exists = await new Query().SelectSingle<int>($"SELECT isnull(1,0) FROM sys.procedures WHERE Name = 'CREATEMODEL'");

            if (exists.Result == 0)
            {
                var CREATEMODEL = @"CREATE PROCEDURE [dbo].[CREATEMODEL]  
                (  
                     @TableName SYSNAME ,  
                     @CLASSNAME VARCHAR(500)   
                )
                AS  
                BEGIN  
                    DECLARE @Result VARCHAR(MAX)  

                    SET @Result = @CLASSNAME + @TableName + '  
                {'  

                SELECT @Result = @Result + '  
                    public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }'  
                FROM  
                (  
                    SELECT   
                        REPLACE(col.NAME, ' ', '_') ColumnName,  
                        column_id ColumnId,  
                        CASE typ.NAME   
                            WHEN 'bigint' THEN 'long'  
                            WHEN 'binary' THEN 'byte[]'  
                            WHEN 'bit' THEN 'bool'  
                            WHEN 'char' THEN 'string'  
                            WHEN 'date' THEN 'DateTime'  
                            WHEN 'datetime' THEN 'DateTime'  
                            WHEN 'datetime2' then 'DateTime'  
                            WHEN 'datetimeoffset' THEN 'DateTimeOffset'  
                            WHEN 'decimal' THEN 'decimal'  
                            WHEN 'float' THEN 'float'  
                            WHEN 'image' THEN 'byte[]'  
                            WHEN 'int' THEN 'int'  
                            WHEN 'money' THEN 'decimal'  
                            WHEN 'nchar' THEN 'char'  
                            WHEN 'ntext' THEN 'string'  
                            WHEN 'numeric' THEN 'decimal'  
                            WHEN 'nvarchar' THEN 'string'  
                            WHEN 'real' THEN 'double'  
                            WHEN 'smalldatetime' THEN 'DateTime'  
                            WHEN 'smallint' THEN 'short'  
                            WHEN 'smallmoney' THEN 'decimal'  
                            WHEN 'text' THEN 'string'  
                            WHEN 'time' THEN 'TimeSpan'  
                            WHEN 'timestamp' THEN 'DateTime'  
                            WHEN 'tinyint' THEN 'byte'  
                            WHEN 'uniqueidentifier' THEN 'Guid'  
                            WHEN 'varbinary' THEN 'byte[]'  
                            WHEN 'varchar' THEN 'string'  
                            ELSE 'UNKNOWN_' + typ.NAME  
                        END ColumnType,  
                        CASE   
                            WHEN col.is_nullable = 1 and typ.NAME in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier')   
                            THEN '?'   
                            ELSE ''   
                        END NullableSign  
                    FROM SYS.COLUMNS col join sys.types typ on col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id  
                    where object_id = object_id(@TableName)  
                ) t  
                ORDER BY ColumnId  
                SET @Result = @Result  + '  
                }'  

                select @Result  

                END ";

                var qresult = await new Query().Execute(CREATEMODEL);
                if (qresult.HasError)
                {
                    response.Code = 0;
                    response.HasError = qresult.HasError;
                    response.Message = qresult.ErrorMessage;
                    return response;
                }

            }

            var projectName = Assembly.GetEntryAssembly().GetName().Name;
            var objectAttributes = (await new Query().SelectSingle<string>($"EXEC [dbo].[CREATEMODEL] @TableName = {ModelName},@CLASSNAME = N'public class '")).Result;
            StringBuilder ModelFile = new StringBuilder();
            ModelFile.AppendLine("using System;")
            .AppendLine(" ")
            .AppendLine($"namespace {projectName}.Custom.Models.{ModelName}")
            .AppendLine("{");
            ModelFile.AppendLine(objectAttributes)
            .AppendLine("}");

            try
            {
                var rootPath = Environment.CurrentDirectory;
                var dir = Directory.CreateDirectory($"{rootPath}\\Custom\\Models\\{ModelName}");


                //Write file using StreamWriter
                using (StreamWriter writer = new StreamWriter($"{rootPath}\\Custom\\Models\\{ModelName}\\{ModelName}.cs"))
                {
                    writer.Write(ModelFile.ToString());
                }

            }
            catch (Exception e)
            {
                response.Code = 0;
                response.HasError = true;
                response.Message = e.Message;

                return response;
            }

            response.Code = 1;
            response.HasError = false;
            response.Message = "Model Created Succsefully";

            return response;
        }


    }


    }