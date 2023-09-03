import React from 'react';
import Footer from '../Common/Footer';
  // import normalB from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/normalBevelled.jpg"
  // import normalM from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/normalM.jpg"
  // import bevelledG from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/bevelledG.jpg"
  // import circleLed from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/circleLed.jpg"
  // import lines from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/blackLines.jpg"
  // import blackB from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/blackB.jpg";
  // import roomB from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/roomB.jpg"
  // import creme from "C:/Users/Admin/Desktop/glass/goblen/src/components/Products/images/cremeFrame.jpg";
const News = () => {
  const news = [
    {
      title: 'Framed Line Mirror',
      description: 'Framed Mirror engraved with black square lines. Choose your custom frame.',
      price: 99.99,
      //image: lines,
    },
    {
      title: 'Diamond Mirror',
      description: 'Diamond shaped bevelled mirror. Custom dimensions for your room.',
      price: 79.99,
      //image: roomB,
    },
    {
      title: 'Half Moon Mirror',
      description: 'Half Moon Mirror with warm LED, an addition to your home. Custom dimensions required.',
      price: 149.99,
      //image: normalM,
    },
    {
      title: 'Bevelled Mirrors',
      description: 'Square Bevelled Mirrors (Normal Color). Sold by custom dimensions',
      price: 179.99,
      //image: normalB,
    },
    {
      title: 'Bevelled Mirrors',
      description: 'Square Bevelled Mirrors (Gold Color). Sold by custom dimensions',
      price: 199.99,
      //image: bevelledG,
    },
    {
      title: 'Custom Bevelling',
      description: 'One of the shapes we created with bevelled mirrors, you say it we create it.',
      price: 209.99,
      //image: blackB,
    },
    {
      title: 'LED Mirror',
      description: 'LED Mirror is the new trend taking by storm. We can add the warmth of the LED by choice',
      price: 249.99,
      //image: circleLed,
    },
    {
      title: 'Creme Framed Mirror',
      description: 'Our best seller of the last 10 years, great addition to your table. Custom dimensions on it as well.',
      price: 169.99,
      //image: creme,
    },
  ];
 
  return (
    <div className="min-h-screen bg-gray-900">
      <div className="py-12">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {news.map((news, index) => (
              <div
                key={index}
                className="bg-white overflow-hidden shadow-xl sm:rounded-lg transform transition-transform duration-300 hover:scale-105"
              >
                <div className="aspect-w-1 aspect-h-1">
                  <img
                    src={news.image}
                    alt={news``.title}
                    className="object-cover object-center w-full h-full"
                  />
                </div>
                <div className="p-6">
                  <h2 className="text-2xl font-semibold mb-2">{news.title}</h2>
                  <p className="text-gray-600 mb-4">{news.description}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};


export default News;
