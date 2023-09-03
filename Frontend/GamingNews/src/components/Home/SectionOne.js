import React from 'react';
import Navigation from './Navigation';

export default function SectionOne() {
  return (
    <div className="bg-gray-900 px-8 md:px-16 lg:px-32">
      {/* Section 1 */}
      <div id="section1" className="bg-gray-900 flex flex-col justify-center items-center h-screen">
        <div className=" mx-auto max-w-2xl py-16 sm:py-24 lg:py-32">
          <div className="hidden sm:mb-8 sm:flex sm:justify-center">
            <div className="relative rounded-full px-3 py-1 text-sm leading-6 text-white ring-1 ring-gray-900/10 hover:ring-gray-900/20">
              Services that we offer.{' '}
              <a href="#" className="font-semibold text-red-600">
                <span className="absolute inset-0" aria-hidden="true" />
                Read more <span aria-hidden="true">&rarr;</span>
              </a>
            </div>
          </div>
          <div className="text-center">
            <h1 className="text-4xl font-bold tracking-tight text-white sm:text-6xl">
              Gaming News <br />
              <span className="text-red-700 text-3xl drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">Since 1970</span>
            </h1>
            <p className="mt-6 text-lg leading-8 text-white">
              Creating news with transparency, our studio transforms raw information into intricate masterpieces that captivate and illuminate the gamers mind. One step ahead for creating services and next-gen flow of the gaming world.
            </p>
            <div className="mt-10 flex items-center justify-center gap-x-6">
              <a
                href="#"
                className="rounded-md bg-gray-600 px-4 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-red-900 focus:outline-none focus:ring focus:ring-red-600"
              >
                Products
              </a>
              <a href="#" className="text-sm font-semibold leading-6 text-red-600 border-black">
                Learn more <span aria-hidden="true">→</span>
              </a>
            </div>
          </div>
        </div>
       <div className='mb-10'><Navigation target="section2" direction="down" /></div>
      </div>
    </div>
  );
}
