const { src, dest } = require('gulp');

exports.default = function() {
  return src('public/**.ico')
    .pipe(dest('build/'));
}