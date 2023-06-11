using Kendo.Mvc.UI;
using GamingWeb.Custom.Models.Kendo.DropDownTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamingWeb.Custom.Helpers
{
    public class DropDownTreeHelper
    {
        public List<DropDownTreeItemModel> GetTreeRecursive(DropDownTreeItemModel parent, List<DropDownTreeData> hierarchicalData, bool parentClickable = true)
        {
            var hierarchy = new List<DropDownTreeItemModel>();
            foreach (var item in hierarchicalData.Where(x => x.IDParent == parent.Id))
            {
                var child = new DropDownTreeItemModel();
                child.Id = item.ID.ToString();
                child.Value = item.ID.ToString();
                child.Text = item.Title;
                //child.SpriteCssClass = "click-disable";
                if ((child.HasChildren = hierarchicalData.Any(x => x.IDParent == child.Id)) && !parentClickable)
                    child.SpriteCssClass = "click-disable";

                if (child.Id != null)
                {
                    child.Items = GetTreeRecursive(child, hierarchicalData);
                    hierarchy.Add(child);
                }
            }
            return hierarchy;
        }

        public List<string> existingID = new List<string>();
        public List<DropDownTreeItemModel> GetTreeRecursivePlanning(DropDownTreeItemModel parent, List<DropDownTreeData> hierarchicalData, bool parentClickable = true, bool firstCall = true)
        {
            try
            {
                var hierarchy = new List<DropDownTreeItemModel>();
                foreach (var item in hierarchicalData.Where(x => x.IDParent == parent.Id || firstCall))
                {
                    var child = new DropDownTreeItemModel();
                    child.Id = item.ID.ToString();
                    child.Value = item.ID.ToString();
                    child.Text = item.Title;
                    //child.SpriteCssClass = "click-disable";
                    if ((child.HasChildren = hierarchicalData.Any(x => x.IDParent == child.Id)) && !parentClickable)
                        child.SpriteCssClass = "click-disable";

                    if (child.Id != null)
                    {
                        child.Items = GetTreeRecursivePlanning(child, hierarchicalData, firstCall: false);
                        if (!firstCall)
                            existingID.Add(item.ID);
                        hierarchy.Add(child);
                    }
                }
                if (firstCall)
                    return hierarchy.Where(x => !existingID.Contains(x.Id)).ToList();
                else
                    return hierarchy;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public List<DropDownTreeModel> GetTreeRecursiveDatasource(int? id, List<TreeDatasource> hierarchicalData)
        //{
        //    var hierarchy = new List<DropDownTreeModel>();
        //    foreach (var item in hierarchicalData.Where(x => x.IDParent == id))
        //    {
        //        var child = new DropDownTreeModel();
        //        child.id = item.ID;
        //        child.Name = item.Title;
        //        child.hasChildren = hierarchicalData.Any(x => x.IDParent == child.id);
        //        hierarchy.Add(child);
        //    }
        //    return hierarchy;
        //}
    }
}
