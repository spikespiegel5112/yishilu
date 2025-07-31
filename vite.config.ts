import { defineConfig } from "vite";
import path from "path";
import vue from "@vitejs/plugin-vue";
import copyPlugin from "rollup-plugin-copy";
import AutoImport from "unplugin-auto-import/vite";
import Components from "unplugin-vue-components/vite";
import ViteComponents from "unplugin-vue-components/vite";
import legacy from "@vitejs/plugin-legacy";

import requireTransform from "vite-plugin-require-transform";

const pathSrc = path.resolve(__dirname, "src");

// https://vitejs.dev/config/
export default defineConfig({
  base: "./",
  plugins: [
    vue({
      script: {
        defineModel: true,
      },
    }),
    AutoImport({
      imports: ["vue", "vuex", "vue-router"],
      dts: "src/auto-import.d.ts",
      resolvers: [],
    }),
    Components({}),
    ViteComponents({}),
    // legacy({
    //   targets: ["ie >= 11", "chrome >= 130"],
    //   additionalLegacyPolyfills: ["regenerator-runtime/runtime"], // 可选
    //   // 下面是其他选项
    //   polyfills: ["es.promise", "es.symbol"], // 指定 polyfills
    //   modernPolyfills: true, // 添加现代浏览器所需的 polyfills
    //   // corejs: 3, // core-js 版本号
    // }),
    requireTransform(),
  ],
  resolve: {
    alias: {
      "~/": `${pathSrc}/`,
      "@/": `${pathSrc}/`,
    },
  },
  css: {
    preprocessorOptions: {
      scss: {
        api: "modern", // or "modern"
        // sassOptions: {
        //   // 消除启动时对过时sass API使用的报警
        //   silenceDeprecations: ["legacy-js-api"],
        // },
      },
    },
  },
  define: {
    "process.env": {},
  },
});
