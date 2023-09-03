import React from 'react';
import Navigation from './Navigation';

const SectionThree = () => {
  return (
    <section className='bg-gray-900'>
      <div class="bg-gray-900 text-white mx-auto max-w-screen-xl px-4 py-16 sm:px-6 sm:py-24 lg:px-8 pb-10">
        <div class="max-w-3xl">
          <h2 class="text-3xl font-bold sm:text-4xl">
          Level Up Your Gaming Experience with the Latest News and Services
          </h2>
        </div>

        <div class="mt-8 grid grid-cols-1 gap-8 lg:grid-cols-2 lg:gap-16">
          <div class="relative h-64 overflow-hidden sm:h-80 lg:h-full">
            <img
              alt="Party"
              src="https://images.unsplash.com/photo-1496843916299-590492c751f4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1771&q=80"
              class="absolute inset-0 h-full w-full object-cover"
            />
          </div>

          <div class="lg:py-16">
            <article class="space-y-4 text-white">
              <p>
              Welcome to Gaming News, your ultimate hub for everything gaming-related! Whether you're a dedicated gamer or just a casual player, we've got you covered. Dive into a world of exciting gaming news, discover upcoming releases, explore in-depth reviews, and stay updated on esports tournaments.
              </p>

              <p>
              But that's not all - our platform also offers a range of services to enhance your gaming journey. From finding the best gaming gear to connecting with like-minded gamers, Gaming News is your go-to destination. Join our community of passionate gamers and level up your gaming experience today! With our expert guides, lively forums, and exclusive deals, you're in for an immersive gaming adventure like never before.
              </p>
            </article>
          </div>
        </div>
      </div>
      
      <div class="flex justify-center pb-21"> 
      <Navigation target="section4" direction="down" />
      </div>
    </section>
  );
};

export default SectionThree;
