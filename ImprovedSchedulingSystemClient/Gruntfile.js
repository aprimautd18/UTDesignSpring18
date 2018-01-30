//Wrapper function with one parameter
module.exports = function(grunt) {
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
        build: {
            files: [{
                expand: true,
                src: '*.js',
                dest: 'app/lib/js',
                cwd: 'build/lib/js/',
            ext: '.min.js'
            }]
          },
        },
        concat: {
            libraries : {
              src : ['app/bower_components/angular/angular.js', 'app/bower_components/angular-route/angular-route.js'],
              dest : 'build/lib/js/libraries.js',
            },
            app: {
                src : ['app/**/*.js', '!app/bower_components/**/*.js'],
                dest : 'build/lib/js/app.js',
            }
        }
        
      });

      grunt.loadNpmTasks('grunt-string-replace');
      grunt.loadNpmTasks('grunt-contrib-concat');
      grunt.loadNpmTasks('grunt-contrib-uglify');
      grunt.registerTask('default', ['concat', 'uglify', 'string-replace']);
    };