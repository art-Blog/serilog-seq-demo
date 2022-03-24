const path = require("path");
const glob = require("glob");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const BomPlugin = require("webpack-utf8-bom");
const FriendlyErrorsWebpackPlugin = require("friendly-errors-webpack-plugin");
const ProgressBarPlugin = require("progress-bar-webpack-plugin");
const SvgSpriteLoaderPlugin = require("svg-sprite-loader/plugin");
const { VueLoaderPlugin } = require("vue-loader");
const webpack = require("webpack");
const { npm_lifecycle_event, NODE_ENV } = process.env;
const isProduction = NODE_ENV === "production";
const entryFiles = glob.sync(path.join(__dirname, "Src/Containers/*/**.js"));
const entryViews = glob.sync(path.join(__dirname, "Views/*/**.cshtml"));

/**
 * @class getEntry
 * @description 動態分析 Containers 入口文件並打包
 */
function getEntry() {
    const Entries = {};
    entryFiles.forEach(function (filePath) {
        const path = /.*\/Containers\/(.*?)\.js/.exec(filePath)[1];
        const bundleName = path.split("/")[0];
        Entries[bundleName] ? Entries[bundleName].push(filePath) : (Entries[bundleName] = [filePath]);
    });
    return Entries;
}

/**
 * @class SetHtmlWebpackPlugin
 * @description 動態生成 CSHTML 頁面，並引入該頁相關 Bundle 檔案
 */
function setHtmlTemplates() {
    const ignoreList = ["Bundle", "BundleTemp", "Shared"];
    const getPath = FileName => "../../Views/Bundle/_" + FileName + ".cshtml";
    entryViews.forEach(filePath => {
        const path = /.*\/Views\/(.*?)\.cshtml/.exec(filePath)[1];
        const [controllerName, actionName] = path.split("/");
        if (ignoreList.includes(controllerName)) return;
        const bundleName = `${controllerName}_${actionName}`;
        const JsTemplatePath = "Views/BundleTemp/_Js_Bundle.cshtml";
        const JsFileNamePath = getPath(bundleName + "_Js");
        commonConfig.plugins.push(
            new HtmlWebpackPlugin({
                template: JsTemplatePath,
                filename: JsFileNamePath,
                chunks: [bundleName],
                inject: false,
            })
        );
        const CssTemplatePath = "Views/BundleTemp/_Css_Bundle.cshtml";
        const CssFileNamePath = getPath(bundleName + "_Css");
        commonConfig.plugins.push(
            new HtmlWebpackPlugin({
                template: CssTemplatePath,
                filename: CssFileNamePath,
                chunks: [bundleName],
                inject: false,
            })
        );
    });
}
const commonConfig = {
    entry: getEntry(),
    optimization: {
        splitChunks: {
            minSize: 30000,
            maxAsyncRequests: 1,
            automaticNameDelimiter: "-",
            cacheGroups: {
                common: {
                    name: "Commons",
                    chunks: "initial",
                    minChunks: 4,
                    priority: 3,
                    test(module, chunks) {
                        return module.resource;
                    },
                },
                style: {
                    name: "Commons",
                    test: /\.(css|scss)$/,
                    chunks: "all",
                    minChunks: 3,
                    reuseExistingChunk: true,
                    enforce: true,
                },
                vendor: {
                    name: "Vendor",
                    test: /(node_modules).*(?<!\.css)$/,
                    chunks: "initial",
                    priority: 3,
                },
            },
        },
        runtimeChunk: { name: "Runtime" },
    },
    resolve: {
        modules: [path.join(__dirname, "node_modules")],
        alias: {
            "~Src": path.join(__dirname, "Src"),
            "~Api": path.join(__dirname, "Src/Api"),
            "~Components": path.join(__dirname, "Src/Components"),
            "~Containers": path.join(__dirname, "Src/Containers"),
            "~Images": path.join(__dirname, "Src/Images"),
            "~Svg": path.join(__dirname, "Src/Svg"),
            "~Styles": path.join(__dirname, "Src/Styles"),
            "~Utils": path.join(__dirname, "Src/Utils"),
            "~Vue": path.join(__dirname, "Src/Vue"),
            "~Vuex": path.join(__dirname, "Src/Vuex"),
        },
        extensions: [".vue", ".js", ".scss", ".sass", ".css"],
    },
    module: {
        rules: [
            {
                test: /\.js/,
                exclude: /node_modules/,
                include: path.join(__dirname, "Src/Utils/utils.js"),
                use: [
                    {
                        loader: "expose-loader",
                        options: {
                            exposes: ["utils"],
                        },
                    },
                ],
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                include: [path.join(__dirname, "Src")],
                use: ["babel-loader"],
            },
            { test: /\.vue$/, use: "vue-loader" },
            {
                test: /\.html$/,
                exclude: /node_modules/,
                include: path.join(__dirname, "Views"),
                use: [
                    {
                        loader: "ejs-loader",
                    },
                ],
            },
            {
                test: /\.(png|svg|jpg|gif|ttf|eot|ico|woff)$/,
                exclude: [path.join(__dirname, "Src/Svg")],
                use: [
                    {
                        loader: "url-loader",
                        options: {
                            limit: 8000,
                            name: "/Images/[name]-[hash:8].[ext]",
                            publicPath: "",
                        },
                    },
                    {
                        loader: "image-webpack-loader",
                        options: {
                            bypassOnDebug: true,
                        },
                    },
                ],
            },
            {
                test: /Svg\\.*\.svg$/,
                use: [
                    {
                        loader: "svg-sprite-loader",
                        options: {
                            extract: true,
                            publicPath: `/${isProduction ? "Bundle" : "__BundleDev__"}/Images/`,
                            outputPath: "Images/",
                            spriteFilename: filePath => `svg-sprite.svg${isProduction ? "?v=[contenthash]" : ""}`,
                        },
                    },
                ],
            },
            {
                test: /Svg\\.*-svgo\.svg$/,
                use: [
                    {
                        loader: "svgo-loader",
                        options: {
                            configFile: path.join(__dirname, "svgo.config.js"),
                        },
                    },
                ],
            },
        ],
    },
    plugins: [
        new BomPlugin(true, /\.(cshtml)$/),
        new FriendlyErrorsWebpackPlugin(),
        new ProgressBarPlugin({
            format: "Webpack build [:bar]" + " :current%" + " :msg ",
            complete: "=",
            incomplete: "-",
            width: 30,
        }),
        new SvgSpriteLoaderPlugin(),
        new VueLoaderPlugin(),
    ],
    stats: "errors-only",
};
if (npm_lifecycle_event !== "dev") {
    // 產生 Bundle HTML
    setHtmlTemplates();
}
module.exports = commonConfig;
