import mImageCrop from './m-image-crop.vue';

export default {
  version: '0.0.1',
  install: Vue => {
    Vue.component(mImageCrop.name, mImageCrop);
  }
};
