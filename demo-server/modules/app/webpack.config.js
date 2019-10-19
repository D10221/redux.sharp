const webpack = require("webpack");
const { resolve } = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const WebpackPwaManifest = require("webpack-pwa-manifest");
const outputPath = resolve(__dirname, "build");
const config = {
  entry: "./src/index.tsx",
  output: {
    path: outputPath,
    filename: "[name].js",
    publicPath: "",
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        use: "babel-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.css$/,
        use: ["style-loader", "css-loader"],
      },
      {
        test: /\.(ts|tsx)?$/,
        loader: "ts-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.svg$/,
        use: "file-loader",
      },
      {
        test: /\.png$/,
        use: [
          {
            loader: "url-loader",
            options: {
              mimetype: "image/png",
            },
          },
        ],
      },
    ],
  },
  resolve: {
    extensions: [".js", ".jsx", ".tsx", ".ts"],
  },
  devServer: {
    contentBase: "./build",
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: resolve(__dirname, "public/body.html"), 
      inject: true,
      appMountId: "root",
      filename: "body.html"
    }),
    new HtmlWebpackPlugin({
      template: resolve(__dirname, "public/head.html"), 
      inject: true,
      filename: "head.html",
      favicon: "public/favicon.ico",
      excludeChunks: ['runtime', 'vendors', 'main'],      
    }),
    new HtmlWebpackPlugin({
      template: require('html-webpack-template'),
      filename: "index.html",
      favicon: "public/favicon.ico",
      appMountId: "root",
      inject: false
    }),
    // TODO: include index.html ... for dev
    new WebpackPwaManifest({
      filename: "manifest.json",
      name: 'MyApp',
      short_name: 'MyApp',
      description: 'MyApp !',
      background_color: '#ffffff',
      // crossorigin: 'use-credentials', //can be null, use-credentials or anonymous
      icons: [
        {
          src: resolve(__dirname, 'public/logo512.png'),
          sizes: [64, 32, 24, 16] // multiple sizes
        },
        {
          src: resolve('public/logo512.png'),
          size: '1024x1024' // you can also use the specifications pattern
        }
      ]
    })
  ],
  optimization: {
    runtimeChunk: "single",
    splitChunks: {
      cacheGroups: {
        vendor: {
          test: /[\\\/]node_modules[\\\/]/,
          name: "vendors",
          chunks: "all",
        },
      },
    },
  },
};

module.exports = config;
