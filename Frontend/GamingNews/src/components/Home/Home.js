import React from 'react';
import SectionOne from './SectionOne';
import SectionTwo from './SectionTwo';
import SectionThree from './SectionThree';
import SectionFour from './SectionFour';

const Home = () => {
  return (
    <div>
      <div id="section1">
        <SectionOne />
      </div>

      <div id="section2">
        <SectionTwo />
      </div>

      <div id="section3">
        <SectionThree />
      </div>

      <div id="section4">
        <SectionFour />
      </div>
    </div>
  );
};

export default Home;
