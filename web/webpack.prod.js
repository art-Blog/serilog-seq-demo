const path = require('path')
const {merge } = require('webpack-merge')
const commonConfig = require('./webpack.common.js')
const TerserPlugin = require('terser-webpack-plugin')
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const ImageminPlugin = require('imagemin-webpack-plugin').default
const { npm_lifecycle_event, NODE_ENV } = process.env
const isProduction = NODE_ENV === 'production'

if (npm_lifecycle_event === 'build:analyzer') {
    commonConfig.plugins.push(new BundleAnalyzerPlugin())
}

module.exports = merge (commonConfig, {
    mode: 'production',
    output: {
        filename: '[name].js?v=[contenthash]',
        chunkFilename: '[name].js?v=[contenthash]',
        path: path.join(__dirname, 'wwwroot/Bundle'),
        publicPath: '/Bundle/',
        library: 'Components'
    },
    module: {
        rules: [
            {
                test: /\.(css|scss)$/,
                use: [
                    { loader: MiniCssExtractPlugin.loader },
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true,
                            modules: true
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
            filename: 'Css/[name].css?v=[contenthash]',
            chunkFilename: 'Css/[name].css?v=[contenthash]'
        }),
        new CleanWebpackPlugin(),
        new OptimizeCssAssetsPlugin(),
        /**
         * 壓縮 svg-sprite-loader 處理後的 svg 檔
         * 利用 svgo 原先就提供的壓縮
         */
        new ImageminPlugin()
    ],
    optimization: {
        minimize: true,
        minimizer: [
            new TerserPlugin({
                parallel: true,
                exclude: /node_modules/,
                extractComments: false,
                terserOptions: {
                    mangle: isProduction,
                    keep_classnames: true,
                    keep_fnames: false,
                    toplevel: false,
                    nameCache: null,
                    ie8: false,
                    output: {
                        beautify: false
                    },
                    compress: {
                        drop_console: isProduction
                    }
                }
            })
        ],
        concatenateModules: true
    }
})
