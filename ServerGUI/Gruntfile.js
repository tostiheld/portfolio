module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        clean: {
            all: ["dist/"],
            tmp: ["dist/js/ServerGUI.js",
                  "dist/css/ServerGUI.css",
                  "dist/index.tmp.html",
                  "dist/css/ServerGUI.min.css",
                  "dist/css/vendor.css",
                  "dist/css/vendor.min.css"],
            html: ["dist/index.tmp.html"],
            css: ["dist/css/ServerGUI.css",
                  "dist/css/ServerGUI.min.css",
                  "dist/css/vendor.css",
                  "dist/css/vendor.min.css"],
            js: ["dist/js/ServerGUI.js"]
        },
        includes: {
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
        concat: {
            options: {
                // define a string to put between each file in the concatenated output
                separator: ';'
            },
            js: {
                // the files to concatenate
                src: ['src/assets/js/**/*.js'],
                // the location of the resulting JS file
                dest: 'dist/js/<%= pkg.name %>.js'
            },
            css: {
                // the files to concatenate
                src: ['src/assets/css/*.css'],
                // the location of the resulting JS file
                dest: 'dist/css/<%= pkg.name %>.css'
            },
            cssVendor: {
                // the files to concatenate
                src: ['dist/css/<%= pkg.name %>.min.css', 'dist/css/vendor.min.css'],
                // the location of the resulting JS file
                dest: 'dist/css/<%= pkg.name %>_vendor.min.css'
            }
        },
        uglify: {
            dist: {
                files: {
                    'dist/js/<%= pkg.name %>.min.js': ['<%= concat.js.dest %>']
                }
            }
        },
        cssmin: {
            main: {
                files: {
                    'dist/css/<%= pkg.name %>.min.css': ['<%= concat.css.dest %>']
                },
                options: {
                    keepSpecialComments: 0
                }
            },
            vendor: {
                files: {
                    'dist/css/vendor.min.css': ['dist/css/vendor.css']
                },
                options: {
                    keepSpecialComments: 0
                }
            }
        },
        uncss: {
            dist: {
                files: {
                    'dist/css/vendor.css': ['dist/index.tmp.html']
                }
            },
            watch: {
                files: {
                    'dist/css/vendor.css': ['dist/index.html']
                }
            },
            options: {
                stylesheets: ["../src/assets/css/vendor/bootstrap.min.css"],
                ignore: ['.modal-open',
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
        htmlmin: {
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
        jshint: {
            // define the files to lint
            files: ['gruntfile.js', 'src/assets/js/**/*.js'],
            options: {
                globals: {
                    jQuery: true,
                    console: true,
                    module: true
                }
            }
        },
        copy: {
            dist: {
                expand: true,
                cwd: 'src/assets/css/vendor/fonts/',
                src: '*',
                dest: 'dist/fonts/',
            },
        },
        watch: {
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


    grunt.registerTask('default', ['jshint', 'clean:all', 'concat:js', 'concat:css', 'cssmin:main', 'includes', 'uncss:dist', 'cssmin:vendor', 'uglify', 'htmlmin', 'concat:cssVendor', 'copy', 'clean:tmp']);

};