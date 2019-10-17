const webpack = require("webpack");
const path = require("path");

const config = [
  {
    name: "client",
    entry: {
      index: "./src/index.tsx",
    },
    output: {
      path: path.resolve(__dirname, "build"),
      filename: "[name].js",
    },
    module: {
      rules: [
        {
          test: /\.css$/,
          use: ["style-loader", "css-loader"],
        },
        {
          test: /\.(ts|tsx)?$/,
          loader: "ts-loader",
          exclude: /node_modules/,
          options: {
            context: __dirname,
            configFile: "tsconfig.build.json",
          },
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
      extensions: [".tsx", ".ts", ".js"],
    },
  },
  // {
  //   name: "server",
  //   entry: "./src/render.js",
  //   target: "node",
  //   output: {
  //     path: path.resolve(__dirname, "build"),
  //     filename: "[name].js",
  //   },
  // },
];

module.exports = config;
