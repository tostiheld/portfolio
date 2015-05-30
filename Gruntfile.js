module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        clean: {
            all: ["dist/"], //Clean whole folder
            tmp: ["dist/js/ServerGUI.js", //clean only temporary files. This will be called at the end of all tasks
                  "dist/css/ServerGUI.css",
                  "dist/index.tmp.html",
                  "dist/css/ServerGUI.min.css",
                  "dist/css/vendor.css",
                  "dist/css/vendor.min.css"],
            html: ["dist/index.tmp.html"], // clean only tmp html files (used for watch)
            css: ["dist/css/ServerGUI.css", // clean only tmp css files (used for watch)
                  "dist/css/ServerGUI.min.css",
                  "dist/css/vendor.css",
                  "dist/css/vendor.min.css"],
            js: ["dist/js/ServerGUI.js"] // clean only js html files (used for watch)
        },
        includes: { // Compiles all the partials into one html file
            files: {
                src: ['src/index.html'],
                dest: 'dist/index.tmp.html',
                flatten: true,
                cwd: '',
                options: {
                    silent: false
                }
            }
        },
        concat: { //concats files
            options: {
                separator: ';'
            },
            js: { //concat all javascript files 
                src: ['src/assets/js/**/*.js'],
                dest: 'dist/js/<%= pkg.name %>.js'
            },
            css: { //concat all main css files
                src: ['src/assets/css/*.css'],
                dest: 'dist/css/<%= pkg.name %>.css'
            },
            cssVendor: { //concat the main css files with the extracted vendor css
                src: ['dist/css/<%= pkg.name %>.min.css', 'dist/css/vendor.min.css'],
                dest: 'dist/css/<%= pkg.name %>_vendor.min.css'
            }
        },
        uglify: { //uglify the javascript!
            dist: {
                files: {
                    'dist/js/<%= pkg.name %>.min.js': ['<%= concat.js.dest %>']
                }
            }
        },
        cssmin: {
            main: { //uglify the main css
                files: {
                    'dist/css/<%= pkg.name %>.min.css': ['<%= concat.css.dest %>']
                },
                options: {
                    keepSpecialComments: 0
                }
            },
            vendor: { //uglify the vendor css
                files: {
                    'dist/css/vendor.min.css': ['dist/css/vendor.css']
                },
                options: {
                    keepSpecialComments: 0
                }
            }
        },
        uncss: {
            dist: { //uncss the vendor css
                files: {
                    'dist/css/vendor.css': ['dist/index.tmp.html']
                }
            },
            watch: { //uncss the vendor css (used for watch, because the html file will be in another place)
                files: {
                    'dist/css/vendor.css': ['dist/index.html']
                }
            },
            options: {
                stylesheets: ["../src/assets/css/vendor/bootstrap.min.css"],
                ignore: ['.modal-open', //ignore these styles because they are needed for modals (loaded by javascript)
                         '.modal-backdrop',
                         '.modal-content',
                         '.modal-backdrop.fade',
                         '.modal-backdrop.in',
                         '.fade',
                         '.in',
                         '.fade.in',
                         '.modal.fade .modal-dialog',
                         '.modal.in .modal-dialog',
                         '.modal-open .modal'
                        ]

            }
        },
        htmlmin: { //minify the html
            dist: {
                files: {
                    'dist/index.html': 'dist/index.tmp.html'
                },
                options: {
                    removeComments: true,
                    collapseWhitespace: true,
                    ignoreCustomComments: [
                        /^\s+ko/,
                        /\/ko\s+$/
                      ]
                }
            }
        },
        jshint: { //check the javascript for errors
            files: ['gruntfile.js', 'src/assets/js/**/*.js'],
            options: {
                globals: {
                    jQuery: true,
                    console: true,
                    module: true
                }
            }
        },
        copy: { //copy's the font files needed by bootstrap (glyph icons)
            dist: {
                expand: true,
                cwd: 'src/assets/css/vendor/fonts/',
                src: '*',
                dest: 'dist/fonts/',
            },
            images: {
                expand: true,
                cwd: 'src/assets/images/',
                src: '*',
                dest: 'dist/images/',
            },
            node: {
                expand: true,
                cwd: 'src/',
                src: 'package.json',
                dest: 'dist/',               
            }
        },
        watch: { //watch, runs only the needed tasks for js/css or html
            // can be called with "grunt watch"
            js: {
                files: ['<%= jshint.files %>'],
                tasks: ['jshint', 'concat:js', 'uglify', 'clean:js']
            },
            css: {
                files: ['src/assets/css/*.css'],
                tasks: ['concat:css', 'cssmin:main', 'uncss:watch', 'cssmin:vendor', 'concat:cssVendor', 'clean:css']
            },
            html: {
                files: ['src/index.html', 'src/partials/**/*.html'],
                tasks: ['includes', 'htmlmin', 'clean:html']
            }
        },
        nodewebkit: {
            options: {
                platforms: ['win', 'osx', 'linux'],
                buildDir: './webkitbuilds', // Where the build version of my node-webkit app is saved 
                version: '0.12.1'
            },
            src: ['./dist/**/*'] // Your node-webkit app 
        }
    });

    // Load the plugin that provides the "uglify" task.
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-htmlmin');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-includes');
    grunt.loadNpmTasks('grunt-uncss');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-node-webkit-builder');


    //default task runs with "grunt"
    //while building use "grunt watch": it automatically recompiles the necessary things
    grunt.registerTask('default', ['jshint', 'clean:all', 'concat:js', 'concat:css', 'cssmin:main', 'includes', 'uncss:dist', 'cssmin:vendor', 'uglify', 'htmlmin', 'concat:cssVendor', 'copy', 'clean:tmp', 'nodewebkit']);
    
    grunt.registerTask('webkit', ['nodewebkit']);

};
