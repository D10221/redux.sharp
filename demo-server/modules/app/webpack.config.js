const webpack = require("webpack");
const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

const config = {
  entry: "./src/index.tsx",
  output: {
    path: path.resolve(__dirname, "build"),
    filename: "[name].js",
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
    contentBase: "./dist",
  },
  // see: https://github.com/jantimon/html-webpack-plugin#options
  // Todo: add $HOME_PAGE or $ROOT site to script src
  plugins: [
    new HtmlWebpackPlugin({
      template: path.resolve(__dirname, "public/body.html"), //require('html-webpack-template'),
      inject: true,
      appMountId: "root",
      filename: "body.html",
    }),
    new HtmlWebpackPlugin({
      template: path.resolve(__dirname, "public/index.html"), //require('html-webpack-template'),
      inject: true,
      appMountId: "root",
      filename: "index.html",
    }),
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
