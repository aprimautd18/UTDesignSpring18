//Wrapper function with one parameter
module.exports = function(grunt) {
    grunt.initConfig({
      'string-replace': {
        inline: {
            files: {
                'app/**': 'app/**'
            },
            options: {
                replacements: [
                    {
                        pattern: 'https://seniordesign2018dev.azurewebsites.net',
                        replacement: ''
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
              src : ['app/bower_components/angular/angular.js', 'app/bower_components/angular-loader/angular-loader.js', 'app/bower_components/angular-mocks/angular-mocks.js', 'app/bower_components/angular-route/angular-route.js'],
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
      grunt.registerTask('default', ['string-replace']);
    };
