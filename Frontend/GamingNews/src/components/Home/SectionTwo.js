import React from 'react';
import worker from "C:/Users/Admin/Desktop/Frontend/GamingNews/src/images/worker.jpg";
import Navigation from './Navigation';

const TeamMember = ({ name, role, imageSrc, socialLinks }) => (
     <div className="bg-white flex flex-col items-center p-8 shadow-lg mb-20">
          {/* <img src={imageSrc} alt={name} className="w-48 h-48 rounded-full mb-4 object-cover" /> */}
          <h2 className="text-2xl font-semibold text-gray-800">{name}</h2>
          <p className="text-gray-600 text-lg">{role}</p>
          <div className=" flex mt-4 space-x-4">
               {socialLinks.map((link, index) => (
                    <a
                         key={index}
                         href={link.url}
                         target="_blank"
                         rel="noopener noreferrer"
                         className="text-red-700 hover:text-red-900 transition"
                    >
                         {link.icon}
                    </a>
               ))}
          </div>
     </div>
);

const SectionTwo = () => {
     const teamMembers = [
          {
               name: 'Annamorea Tiara Nixha',
               role: 'CEO',
                imageSrc: worker,
               socialLinks: [
                    { icon: 'Facebook', url: 'https://www.facebook.com/johndoe' },
                    { icon: 'Twitter', url: 'https://twitter.com/johndoe' },
               ],
          },
          {
               name: 'Eldi Vuniqi',
               role: 'Chief Editor',
               imageSrc: worker,
               socialLinks: [
                    { icon: 'LinkedIn', url: 'https://www.linkedin.com/janesmith' },
                    { icon: 'Instagram', url: 'https://www.instagram.com/janesmith' },
               ],
          },
          {
               name: 'Altin Sadiku',
               role: 'Chief Products Manager',
               imageSrc: worker,
               socialLinks: [
                    { icon: 'GitHub', url: 'https://github.com/alexjohnson' },
                    { icon: 'Globe', url: 'https://www.alexjohnson.com' }, // Using a different icon
               ],
          },
          {
               name: 'Art Ejupi',
               role: 'CTO',
               imageSrc: worker,
               socialLinks: [
                    { icon: 'GitHub', url: 'https://github.com/alexjohnson' },
                    { icon: 'Globe', url: 'https://www.alexjohnson.com' }, // Using a different icon
               ],
          },
          {
               name: 'Mark Thorn',
               role: 'Head Writer',
               imageSrc: worker,
               socialLinks: [
                    { icon: 'GitHub', url: 'https://github.com/alexjohnson' },
                    { icon: 'Globe', url: 'https://www.alexjohnson.com' }, // Using a different icon
               ],
          },
          {
               name: 'Alex Johnson',
               role: 'NY TIMES #1 Gaming Top Writer 2023',
               imageSrc: worker,
               socialLinks: [
                    { icon: 'GitHub', url: 'https://github.com/alexjohnson' },
                    { icon: 'Globe', url: 'https://www.alexjohnson.com' }, // Using a different icon
               ],
          }
          
     ];
     return (
          <div id="section2" className="bg-gray-900 min-h-screen flex flex-col justify-center items-center">
               <div className="bg-gray-900 container mx-auto mt-10">
                    <div className="bg-gray-900 text-center mb-20">
                         <h1 className="bg-gray-900 text-4xl font-semibold mb-4 text-white">Meet Our Team</h1>
                         <p className="bg-gray-900 text-white text-lg">Passionate and dedicated individuals behind our success</p>
                    </div>
                    <div className="bg-gray-900 grid grid-cols-1 md:grid-cols-3 gap-12">
                         {teamMembers.map((member, index) => (
                              <TeamMember key={index} {...member} />
                         ))}
                    </div>
               </div>
        <div className='mb-10'><Navigation target="section3" direction="down" /></div>
          </div>
     );
};

export default SectionTwo;
