import { defineConfig } from "vite";
import path from "path";
import vue from "@vitejs/plugin-vue";
import AutoImport from "unplugin-auto-import/vite";
import Components from "unplugin-vue-components/vite";
import ViteComponents from "unplugin-vue-components/vite";
// 使用你所使用的UI组件库的 resolver
// import { ElementPlusResolver } from "unplugin-vue-components/resolvers";

// import Unocss from "unocss/vite";
// import {
//   presetAttributify,
//   presetIcons,
//   presetUno,
//   transformerDirectives,
//   transformerVariantGroup,
// } from "unocss";

const pathSrc = path.resolve(__dirname, "src");

// https://vitejs.dev/config/
export default defineConfig({
  base: "./",
  plugins: [
    vue(),
    ViteComponents({
      resolvers: [ElementPlusResolver()],
      dts: "src/components.d.ts",
    }),
    AutoImport({
      // dts: "types/auto-imports.d.js",
      // imports: ["vue", "vuex", "vue-router"],
      // resolvers: [ElementPlusResolver()],
    }),
    Components({
      // allow auto load markdown components under `./src/components/`
      extensions: ["vue", "md"],
      // allow auto import and register components used in markdown
      include: [/\.vue$/, /\.vue\?vue/, /\.md$/],
      // resolvers: [
      //   ElementPlusResolver({
      //     importStyle: "sass",
      //   }),
      // ],
      dts: "src/components.d.ts",
    }),
  ],
  resolve: {
    alias: {
      "~/": `${pathSrc}/`,
      "@/": `${pathSrc}/`,
    },
  },
  css: {
    preprocessorOptions: {
      scss: {},
    },
  },
  define: {
    "process.env": {},
  },
});
