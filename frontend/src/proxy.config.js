const proxy = [
  {
    context: '/api',
    target: 'http://localhost:80',
    pathRewrite: {'^/api' : ''}
  }
];
module.exports = proxy;
