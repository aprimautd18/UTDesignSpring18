//Wrapper function with one parameter
module.exports = function(grunt) {
    grunt.initConfig({
        concat: {
            libraries : {
              src : ['./app/bower_components/**/*.js'],
              dest : './lib/js/libraries.js',
            },
            app: {
                src : ['./app/**/*.js', '!./app/bower_components/**/*.js'],
                dest : './lib/js/libraries.js',
            }
          }
        }
      });



      // What to do by default. In this case, nothing.
      grunt.registerTask('default', []);
    };