import $ from 'jquery'
// import ion from '../js/ion-sound.js'


var isFunc = function (f) {
  return typeof f === 'function';
}


/* 从此处开始主要逻辑 */

// var ionInstance = new ion();
var ionInstance = {}

//构造器函数
function frame_ani(config) {
  this.option = {
    canvasTargetId: null, // 目标画布对象ID（必须）
    framesUrl: [], // 每一帧的url（必须）
    audioObject: null, // 音频的对象（优先级高于ION）
    audioIonName: null, // ION音频的名字（优先级高于路径）
    audioUrl: "", // 音频路径（优先级最低）
    height: 300, // 图片的高度（必须）
    width: 300, // 图片的宽度（必须）
    slicing: 'auto',
    align: 'top',
    // baseOnWidth:true,
    // baseOnHeight:false,
    onStart: null, // 加载开始回调函数，传入参数total
    onComplete: null, // 播放完毕回调
    onLoading: function () {
    },
    onLoadingFinished: function () {

    },

    loop: false, // 是否循环
    frequency: 25, // 每秒帧数
  }
  // 覆盖默认配置
  if (config) {
    Object.assign(this.option, config)
    // Object.keys(config).forEach(item->{
    //   this.option[i] = config[i];
    // })
    // for (i in config) {
    //
    //
    // }
  }
  else {
    alert('参数错误！');
    return;
  }
  this.status = 0; // 状态，0：未启动   1：正在加载   2：播放中（没写）
  this.total = this.option.framesUrl.length || 0; //资源总数
  this.ctx = document.getElementById(this.option.canvasTargetId).getContext('2d'); // 画布上下文
  // debugger

  var that = this;
  if (this.option.audioObject != null) {
    this.bgm = this.option.audioObject;
  } else if (this.option.audioIonName != null) {
    // this.bgm_ion_name = this.option.audioIonName;
    this.bgm_ion_name = null;
  } else {
    this.bgm = new Audio(); // 背景音乐！
    this.bgm.onerror = function () {
      that.bgm = undefined;
    }
    this.bgm.src = this.option.audioUrl;
  }

  // 设置频率
  // this.frequency = 25;

  // 启动缓存
  // this.initialize();
};

// 预加载
frame_ani.prototype.initialize = function (callback) {
  this.initialize_cb = callback;
  // 启动加载，图片按顺序存起来，加载完毕后开始播放
  this.status = 1;
  var that = this;

  // debugger
  this.frames = []; // 存帧对象的
  this.currentIndex = 0; //当前正在加载的资源索引

  frame_ani.prototype.frameLength = this.option.framesUrl.length;
  for (var i = 0, l = this.option.framesUrl.length; i < l; i++) {
    var url = this.option.framesUrl[i];
    var image = new Image();
    image.onload = function () {

      that.loaded();
      that.onLoading()

      // that.onLoading()
      //
      // if (isFunc(that.option.onLoading)) {
      //   debugger
      // }

    };
    image.onerror = function () {
      console.log('preload error.');
      that.loaded();
    };
    image.src = url;
    this.frames.push(image);
  }
  // 回调一下启动函数
  if (isFunc(this.option.onStart)) {
    this.option.onStart(this.total);
  }
}

// 设置封面
frame_ani.prototype.setPoster = function () {
  this.calculate();
  this.ctx.clearRect(0, 0, this.width, this.height);

  this.ctx.drawImage(this.frames[0],
    this.showX, this.showY, this.sourceWidth, this.sourceHeight,
    0, 0, this.canvasWidth, this.canvasHeight
  );
  // console.log("画好封面了");
  if (isFunc(this.initialize_cb)) {
    this.initialize_cb();
  }
}

// 计算一下宽高和显示位置，默认居中靠底部显示（background-position: center bottom）
// 非默认的没写
frame_ani.prototype.calculate = function () {
  this.canvasWidth = $("#" + this.option.canvasTargetId).width();
  this.canvasHeight = $("#" + this.option.canvasTargetId).height();
  var canvasR = 1.0 * this.canvasWidth / this.canvasHeight; // 窗口的宽高比
  var imageR = 1.0 * this.option.width / this.option.height;


  if (this.option.slicing === 'auto') {

    // 用来计算裁剪的宽高
    if (imageR < canvasR) {
      // 宽度相同，高度要裁剪
      this.width = this.canvasWidth;
      this.height = this.canvasWidth / imageR;

      this.sourceWidth = this.option.width;
      this.sourceHeight = this.option.width / canvasR;
      // this.showX = 0;
      // this.showY = this.option.height - this.sourceHeight;
    } else {
      // 高度相同，宽度要裁剪
      this.width = this.canvasHeight * imageR;
      this.height = this.canvasHeight;

      this.sourceWidth = this.option.height * canvasR;
      this.sourceHeight = this.option.height;

      // this.showX = (this.option.width - this.sourceWidth) / 2;
      // this.showY = 0;
    }

  } else {
    switch (this.option.slicing) {
      case 'widthBased':
        this.width = this.canvasWidth;
        this.height = this.canvasWidth / imageR;

        this.sourceWidth = this.option.width;
        this.sourceHeight = this.option.width / canvasR;
        break;
      case 'heightBased':
        this.width = this.canvasWidth;
        this.height = this.canvasWidth / imageR;

        this.sourceWidth = this.option.width;
        this.sourceHeight = this.option.width / canvasR;
        break;
      default:
        alert('未定义画幅裁切方案')
    }
  }


  switch (this.option.align) {
    case 'top':
      this.showX = (this.option.width - this.sourceWidth) / 2;
      this.showY = 0;
      break;
    case 'bottom':
      this.showX = (this.option.width - this.sourceWidth) / 2;
      this.showY = this.option.height - this.sourceHeight;
      break;
    default:
      this.showX = (this.option.width - this.sourceWidth) / 2;
      this.showY = 0;
      break;
  }


  console.log(this.showX, this.showY, this.canvasWidth, this.canvasHeight);
}

// 播放视频
frame_ani.prototype.play = function () {
  // 设置当前播放帧
  this.currentTimes = this.currentTimes || 0;
  this.maxFramesTimes = this.option.framesUrl.length; // 总帧数
  this.lastTimestamp = undefined;
  // 开始播放
  this.status = 2;
  // 如果有音频也播放
  if (this.bgm != undefined) {
    this.bgm.play();
  }
  if (this.bgm_ion_name) {
    // debugger
    ionInstance.sound.play(this.bgm_ion_name);
  }
  window.requestAnimationFrame(this.nextFrame.bind(this));
}

// 暂停
frame_ani.prototype.pause = function () {
  this.status = 3;
  // bgm 也停一下
  if (this.bgm != undefined) {
    this.bgm.pause();
  }
  if (this.bgm_ion_name) {
    ionInstance.sound.pause(this.bgm_ion_name);
  }
}

// 重置播放序列和音频
frame_ani.prototype.reset = function () {
  this.currentTimes = 0;
  if (this.bgm) {
    this.bgm.currentTime = 0;
  }
  if (this.bgm_ion_name) {
    ionInstance.sound.stop(this.bgm_ion_name);
  }
}

// 绘制下一帧，核心函数？
frame_ani.prototype.nextFrame = function (timestamp) {
  if (this.bgm_ion_name) {
    ionInstance.sound.play(this.bgm_ion_name);
  }
  if (this.status === 3) {
    // 提前结束了
    if (isFunc(this.option.onComplete)) {

      this.option.onComplete();
    }
    // 双重保障？
    if (this.bgm != undefined) {
      this.bgm.pause();
    }
    if (this.bgm_ion_name) {
      ionInstance.sound.stop(this.bgm_ion_name);
    }
    return false;
  }
  // console.log(timestamp);
  var needRedraw = false; // 需要重绘
  if (this.lastTimestamp === undefined) {
    this.lastTimestamp = timestamp;
    this.carryTime = 0; // 上次的余数
    var jumpFrames = 0;
    needRedraw = true;
  } else {
    var jumpFrames = parseInt((timestamp - this.lastTimestamp + this.carryTime) / (1000 / this.option.frequency));
    if (jumpFrames > 0) {
      this.currentTimes += jumpFrames;
      needRedraw = true;
      this.carryTime = (timestamp - this.lastTimestamp + this.carryTime) % (1000 / this.option.frequency);
      this.lastTimestamp = timestamp;
    }
  }

  // console.log("准备画第 "+ this.currentTimes + " 帧");
  // 重绘
  if (needRedraw && this.currentTimes < this.maxFramesTimes) {
    // 疯狂更新来监听窗口大小变化
    // this.calculate();
    // console.log("画了一帧！现在在第：" + this.currentTimes + "帧！");
    this.ctx.clearRect(0, 0, this.width, this.height);
    // console.log(this.canvasWidth, this.canvasHeight);
    this.ctx.drawImage(this.frames[this.currentTimes],
      this.showX, this.showY, this.sourceWidth, this.sourceHeight,
      0, 0, this.canvasWidth, this.canvasHeight
    );
  }

  // 是否继续
  if (this.currentTimes < this.maxFramesTimes) {
    // console.log("下一帧");
    window.requestAnimationFrame(this.nextFrame.bind(this));
  } else if (isFunc(this.option.onComplete)) {
    console.log("没有下一帧");
    this.option.onComplete();
    if (this.option.loop === true) {
      // 需要循环播放
      this.currentTimes = 0;
      this.lastTimestamp = undefined;
      // 音频也从头开始
      if (this.bgm != undefined) {
        this.bgm.currentTime = 0;
        this.bgm.play();
      }
      if (this.bgm_ion_name) {
        console.log('restart ion sound');
        ionInstance.sound.stop(this.bgm_ion_name);
      }
      window.requestAnimationFrame(this.nextFrame.bind(this));
    } else {
      // 结束了
      this.status = 3;
      if (this.bgm != undefined) {
        this.bgm.pause();
      }
      // debugger

      if (this.bgm_ion_name) {
        ionInstance.sound.stop(this.bgm_ion_name);
      }
    }
  }
  // this.currentTimes++;
}

// 计数函数
frame_ani.prototype.loaded = function () {
  this.currentIndex++;

  // console.log(this.currentIndex)
  if (this.currentIndex === this.total) {
    // 加载完毕，设置一下封面美滋滋
    // debugger
    this.setPoster();
    this.onLoadingFinished();
  }
}

frame_ani.prototype.onLoading = function () {
  // console.log(this)
  // debugger
  var index = this.currentIndex;
  this.option.onLoading(this.currentIndex)
}
frame_ani.prototype.onLoadingFinished = function () {
  console.log(this)
  // debugger
  var index = this.currentIndex;
  this.option.onLoadingFinished(this.currentIndex)
}

//暴露粗去
export default frame_ani;
// return frame_ani;
