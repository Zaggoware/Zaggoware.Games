const path = require("path");
const projectRoot = path.resolve(__dirname);

module.exports = {
    configureWebpack: {
        resolve: {
            alias: {
                '@': path.join(projectRoot, "src"),
            }
        },
        plugins: []
    },

    pluginOptions: {
        lintStyleOnBuild: true,
        stylelint: {
        }
    },

    transpileDependencies: [
        'vue-i18n'
    ],

    lintOnSave: 'default',
    productionSourceMap: false
};
