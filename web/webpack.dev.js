const path = require('path')
const { merge  } = require('webpack-merge')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const SpeedMeasurePlugin = require('speed-measure-webpack-plugin')
const SMP = new SpeedMeasurePlugin()
const commonConfig = require('./webpack.common.js')
const { npm_lifecycle_event } = process.env

let config = merge (commonConfig, {
    mode: 'development',
    devtool: 'eval-cheap-module-source-map',
    output: {
        filename: '[name].js',
        path: path.join(__dirname, 'wwwroot/__BundleDev__'),
        publicPath: '/__BundleDev__/',
        library: 'Components'
    },
    module: {
        rules: [
            {
                test: /\.(css|scss)$/,
                use: [
                    'vue-style-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true,
                            modules: {
                                mode: 'local',
                                localIdentName: '[name]_[local]-[hash:base64:5]'
                            }
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap: true
                        }
                    }
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: 'Css/[name].css'
        })
    ]
})

if (npm_lifecycle_event === 'start') {
    config.plugins.push(
        new CleanWebpackPlugin()
    )
}
if (npm_lifecycle_event === 'dev:analyzer') {
    Config.plugins.push(new BundleAnalyzerPlugin())
    Config = SMP.wrap(config)
}
module.exports = config