//Wrapper function with one parameter
module.exports = function(grunt) {

  var name = '-v';

    grunt.initConfig({
      'string-replace': {
        inline: {
            files: {
                'app/index.html': 'app/index.html'
            },
            options: {
                replacements: [
                    {
                        pattern: '<!--start PROD imports',
                        replacement: '<!--start PROD imports-->'
                    },
                    {
                        pattern: 'end PROD imports-->',
                        replacement: '<!--end PROD imports-->'
                    },
                    {
                        pattern: '<!--start DEV imports-->',
                        replacement: '<!--start DEV imports'
                    },
                    {
                        pattern: '<!--end DEV imports-->',
                        replacement: 'end DEV imports-->'
                    }
                ]
            }
        }
    },
      uglify: {
        options: {
          sourceMapRoot: '../',
          sourceMap: 'distrib/'+name+'.min.js.map',
          sourceMapUrl: name+'.min.js.map'
        },
        target : {
          src : ['lib/js/*.js'],
          dest : 'lib/js/' + name + '.min.js'
        }
      },
        concat: {
            libraries : {
              src : ['app/bower_components/**/*.js'],
              dest : 'lib/js/libraries.js',
            },
            app: {
                src : ['app/**/*.js', '!app/bower_components/**/*.js'],
                dest : 'lib/js/app.js',
            }
          }
        }
      });

      grunt.loadNpmTasks('grunt-string-replace');
      grunt.loadNpmTasks('grunt-contrib-concat');
      grunt.loadNpmTasks('grunt-contrib-uglify');
      grunt.registerTask('default', ['concat', 'uglify', 'string-replace']);
    };