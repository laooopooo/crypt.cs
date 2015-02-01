/**
 * Build system.
 * @module gulpfile
 */
'use strict';

// Module dependencies.
var child=require('child_process');
var del=require('del');
var fs=require('fs');
var gulp=require('gulp');
var path=require('path');
var plugins=require('gulp-load-plugins')();
var pkg=require('./package.json');
var util=require('util');

/**
 * Provides tasks for [Gulp.js](http://gulpjs.com) build system.
 * @class Crypt.Gulpfile
 * @static
 */

/**
 * Runs the default tasks.
 * @method default
 */
gulp.task('default', function(callback) {
  _winExec('MSBuild/12.0/Bin/MSBuild.exe', [ '/nologo', '/property:Configuration=Release', '/verbosity:minimal' ], callback);
});

/**
 * Checks the package dependencies.
 * @method check
 */
gulp.task('check', function() {
  return gulp.src('package.json')
    .pipe(plugins.david())
    .pipe(plugins.david.reporter);
});

/**
 * Deletes all generated files and reset any saved state.
 * @method clean
 */
gulp.task('clean', function(callback) {
  var items=[
    'src/**/obj',
    'var/debug',
    'var/release',
    util.format('var/%s-%s.exe', pkg.yuidoc.name.toLowerCase(), pkg.version)
  ];

  del(items, callback);
});

/**
 * Creates a distribution file for this program.
 * @method dist
 */
gulp.task('dist', [ 'default' ], function(callback) {
  _winExec('Inno Setup/ISCC.exe', [ '/qp', 'setup.iss' ], callback);
});

/**
 * Builds the documentation.
 * @method doc
 */
gulp.task('doc', [ 'doc:assets' ], function(callback) {
  _exec('docgen', callback);
});

gulp.task('doc:assets', function() {
  return gulp.src([ 'www/apple-touch-icon.png', 'www/favicon.ico' ])
    .pipe(gulp.dest('doc/api/assets'));
});

/**
 * Performs static analysis of source code.
 * @method lint
 */
gulp.task('lint', [ 'lint:doc', 'lint:js' ]);

gulp.task('lint:doc', function(callback) {
  _exec('docgen --lint', callback);
});

gulp.task('lint:js', function() {
  return gulp.src('*.js')
    .pipe(plugins.jshint(pkg.jshintConfig))
    .pipe(plugins.jshint.reporter('default', { verbose: true }));
});

/**
 * Runs a command and prints its output.
 * @method _exec
 * @param {String} command The command to run, with space-separated arguments.
 * @param {Function} callback The function to invoke when the task is over.
 * @async
 * @private
 */
function _exec(command, callback) {
  child.exec(command, function(err, stdout) {
    console.log(stdout.trim());
    if(err) console.error(err);
    callback();
  });
}

/**
 * Runs a Windows program.
 * @method _winExec
 * @param {String} program The program to run.
 * @param {Array} args The program arguments.
 * @param {Function} callback The function to invoke when the task is over. It is passed one argument `(err)`, where `err` is the error that occurred, if any.
 * @async
 * @private
 */
function _winExec(program, args, callback) {
  if(process.platform!='win32') callback(new Error('Only supported on Windows operating systems.'));
  else {
    var executable=path.join(process.env.ProgramFiles, program);
    fs.exists(executable, function(exists) {
      if(!exists && ('ProgramFiles(x86)' in process.env)) executable=path.join(process.env['ProgramFiles(x86)'], program);
      _exec(util.format('"%s" %s', executable, args.join(' ')), callback);
    });
  }
}
