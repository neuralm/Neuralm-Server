module.exports = {
  // Fix for message type serialization. 
  // It is depended upon the constructor name of the message.
  // Therefore we need to disable minimization..
  chainWebpack: config => config.optimization.minimize(false)
}
