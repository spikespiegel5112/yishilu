"use strict";

// import 'style/style.scss'

function _BaobaoAudioPlayback(options) {
  options = Object.assign(
    {
      src: "",
      loop: false,
      // autoplay: false,
      icon: true,
      el: "body",
      preload: "auto",
    },
    options,
  );

  state.loading = true;
  this.el = options.el;
  this.audioInstance = document.createElement("audio");
  // this.audioInstance = new Audio();
  this.audioInstance.preload = options.preload;
  this.audioInstance.src = options.src;
  this.audioInstance.loop = options.loop;
  this.instanceId = "commonAudio_" + Date.parse(new Date());

  const templateString = "<a></a><p class='loadinghint'>音乐载入中</p>";
  let templateNode = document.createElement("div");
  templateNode.setAttribute("id", this.instanceId);
  templateNode.setAttribute("class", "common_musicbutton_wrapper");
  templateNode.innerHTML = templateString;
  document.querySelector(options.el).appendChild(templateNode);

  if (options.icon) {
    templateNode.classList.add("active");
    templateNode.classList.add("loading");
  } else {
    templateNode.classList.add("hidden");
  }

  this.instanceElement = document.getElementById(this.instanceId);
  let that = this;

  this.play2 = () => {
    return new Promise((resolve, reject) => {
      const play = function () {
        document.getElementById(that.instanceId).classList.remove("loading");
        that.audioInstance
          .play()
          .then((data) => {
            console.log("audioInstance play success");
            resolve(data);
          })
          .catch((error:any) => {
            console.log("audioInstance error", error);
            reject(error);
          });
      };
      if (_BaobaoAudioPlayback.prototype.checkOS() !== "ios") {
        console.log("checkOS", _BaobaoAudioPlayback.prototype.checkOS());
        if (state.loading) {
          this.audioInstance.addEventListener(
            "canplaythrough",
            (e) => {
              play();
              state.loading = false;
            },
            false,
          );
        } else {
          play();
        }
      } else {
        state.loading = false;
        play();
      }
    });
  };

  this.pause = function () {
    this.audioInstance.pause();
  };

  // this.play();

  this.instanceElement.addEventListener("click", () => {
    if (this.audioInstance.paused) {
      this.play2();
      this.instanceElement.classList.add("active");
    } else {
      this.pause();
      this.instanceElement.classList.remove("active");
    }
  });
  console.log("aaa", this.audioInstance);
}

// _BaobaoAudioPlayback.prototype.constructor = _BaobaoAudioPlayback;

_BaobaoAudioPlayback.prototype.checkOS = function () {
  const ua = navigator.userAgent;
  console.log("us", ua);
  let environmentDictionary = [
    {
      //   name: 'wechat',
      //   checker: ua.toLowerCase().match(/MicroMessenger/i),
      //   getter: '',
      //   status: false
      // }, {
      name: "ios",
      method: "window.webkit.messageHandlers.token.postMessage('')",
      checker: !!ua.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/),
      getter: "window.webkit.messageHandlers.token",
      status: false,
    },
    {
      name: "android",
      method: "window.android",
      checker: ua.match(/(Android)\s+([\d]+)/),
      getter: "window.android.getToken()",
      status: false,
    },
  ];
  let environment;
  environmentDictionary.forEach((item:any, index:number) => {
    if (item.checker) {
      environment = item.name;
    }
  });
  let result = environmentDictionary.filter((item) => item.checker);
  environment = result.length > 0 ? result[0].name : "unknown";

  return environment;
};

// const BaobaoAudioPlayback = options => {
//   return new _BaobaoAudioPlayback(options)
// };

export default _BaobaoAudioPlayback;
