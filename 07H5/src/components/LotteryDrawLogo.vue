<template>
	<div class="lotterydraw_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
		</div>
		<!-- <div class="bg"></div> -->
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/interaction/subtitle_00000.png" alt />
			</div>
			<div class="wheel">
				<canvas id="wheelcanvas" :width="remUnit*13.5" :height="remUnit*13.5">抱歉！浏览器不支持。</canvas>
				<a class="begin" @click="drawPrize"></a>
			</div>
			<div class="hint">
				<p>奖品请至依视路展台领取</p>
			</div>
			<div class="common_goback_wrapper">
				<CommonGoBack to="interactionLogo" />
			</div>
		</div>
		<div v-if="dialogThankYouFlag" class="common_dialog_container thankyou">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p>谢谢参与</p>
				</div>
				<div class="otheractivity">
					<router-link :to="{name:'interactionLogo'}">*请点击此处，了解额外互动活动</router-link>
				</div>
			</div>
		</div>
		<div v-if="dialogRunOutFlag" class="common_dialog_container runout">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p>您的抽奖机会已用完</p>
				</div>
			</div>
		</div>
		<div v-if="dialogPrizeFlag" class="common_dialog_container prize">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<div class="title">
						<img src="@/image/lotterydraw/gift_00000.png" alt />
					</div>
					<div class="desc">
						<div class="prizeinfo">
							<p class="degree">恭喜您获得{{prizeData.prize_Name}}</p>
							<p class="prize">{{prizeData.prize_Content}}{{prizeData.unit}}</p>
						</div>
						<p class="hint">{{prizeData.remark}}</p>
					</div>
				</div>
				<div class="otheractivity">
					<router-link :to="{name:'interactionLogo'}">*请点击此处，了解额外互动活动</router-link>
				</div>
			</div>
		</div>
	</div>
</template>

<script>


export default {
	name: "LotteryDraw",
	components: {
	},
	data() {
		return {
			drawlist2Request: 'h5.get.wxuser.drawlist2',
			draw2Request: 'h5.user.luck.draw2',
			dialogThankYouFlag: false,
			dialogPrizeFlag: false,
			dialogRunOutFlag: false,
			status: false,
			wheelCanvas: {},
			remUnit: '',
			alreadyReleasedPrize: false,
			alreadyReceivedPrize: false,
			rotateDuration: 3600,
			colorDictionary: ['#E6E6E6', '#1D88C2'],
			textColorDictionary: ['#1D88C2', '#E6E6E6'],
			dotsColorDictionary: ['#ffd800', '#fe9166'],
			wheelData: [{
				name: '比萨饼',
				value: 10,
				image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png'
			}, {
				name: '酱肘子',
				value: 20,
				image: 'http://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/280px-PNG_transparency_demonstration_1.png'
			}, {
				name: '红烧肉',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}, {
				name: '炖排骨',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}, {
				name: '小鸡炖蘑菇',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}, {
				name: '牛排',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}, {
				name: '小鸡炖蘑菇',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}, {
				name: '牛排',
				value: 30,
				image: 'http://pic.c-ctrip.com/platform/online/home/un_index_supply.png'
			}],
			// userInfo: {},
			prizeData: {}
		};
	},
	computed: {
		navigatorStyle() {
			return {
				'background-image': "url(" + require('@/image/homepage/navigator_00000.png') + ")"
			};
		},
		userInfo() {
			return this.$store.state.userInfo
		}
	},
	watch: {
		remUnit(value) {
			this.$nextTick(() => {
				this.canvasWidth = value * 13.5 + 'px';
				this.canvasHeight = value * 13.5 + 'px';
				this.getPrizeList();
			});
		},
	},

	mounted() {
		setTimeout(() => {
			this.init();

		}, 100);
		this.$nextTick(() => {
			this.remUnit = Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''));
		});
		// this.getUserInfo();
	},
	methods: {
		init() {
			// console.log(FrameAnimation)

		},
		// getUserInfo() {
		// 	this.userInfo = JSON.parse(this.$webStorage.getItem('userInfo'));
		// },
		closeDialog() {
			this.dialogThankYouFlag = false;
			this.dialogRunOutFlag = false
			this.dialogPrizeFlag = false;
		},
		getPrizeList() {
			this.loading = true;
			// this.drawCanvas();
			// this.getCachedCircleNumber();
			this.$http.get(this.$baseUrl + this.drawlist2Request, {
				params: {
					u_id: this.userInfo.id
				}
			}).then(response => {
				console.log('getPrizeList+++++++=', response);
				this.loading = false;
				this.wheelData = [];
				// this.activityInfo = response.activityInfo;

				// if (JSON.parse(sessionStorage.getItem('dailyLimit') === 'null')) {
				// 	this.dailyLimit = response.activityInfo.dailyLimit;
				// }
				response.data.forEach((item, index) => {
					this.wheelData.push({
						name: item.prize_Name,
						// image: item.rewardImage !== null ? item.rewardImage + '-style_100x100' : '',
						// image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png',
						value: item.id,
					});
					// if (response.data.length === 6 && index === 5) {
					// 	for (let index = 0; index < 2; index++) {
					// 		this.wheelData.push({
					// 			name: item.prize_Name,
					// 			// image: item.rewardImage !== null ? item.rewardImage + '-style_100x100' : '',
					// 			// image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png',
					// 			value: item.id,
					// 		});
					// 	}
					// }
				});

				this.drawCanvas();
				this.getCachedCircleNumber();

			}).catch(error => {
				console.log(error);
				this.loading = false;
				this.$vux.confirm.show({
					showCancelButton: false,
					title: error.data.message,
					onConfirm() {
					}
				});
			});
		},
		drawCanvas() {

			console.log(this.remUnit);
			// console.log(this.canvasWidth)
			// console.log(this.wheelData)
			this.wheelCanvas = document.getElementById('wheelcanvas');
			let ctx = this.wheelCanvas.getContext('2d');
			let ctx2 = this.wheelCanvas.getContext('2d');

			let baseAngle = Math.PI * 2 / this.wheelData.length;
			// document.querySelector('.wheel_wrapper .wheel').style.width = this.remUnit * 13.5;
			// document.querySelector('.wheel_wrapper .wheel').style.height = this.remUnit * 13.5;

			let canvasWidth = this.remUnit * 13.5;
			let canvasHeight = this.remUnit * 13.5;
			// console.log(canvasWidth)
			// console.log(canvasHeight)

			ctx.font = this.remUnit;

			// ctx2.beginPath();
			// ctx2.arc(canvasWidth / 2, canvasHeight / 2, this.remUnit * 6.75, 0, Math.PI * 2, true);
			// ctx2.fillStyle = 'rgba(188,75,61,0.5)';
			// ctx2.fill();

			// ctx2.beginPath();
			// ctx2.arc(canvasWidth / 2, canvasHeight / 2, this.remUnit * 6.57, 0, Math.PI * 2, true);
			// ctx2.fillStyle = '#bc4b3d';
			// ctx2.fill();

			// ctx2.beginPath();
			// ctx2.arc(canvasWidth / 2, canvasHeight / 2, this.remUnit * 6.35, 0, Math.PI * 2, true);
			// ctx2.fillStyle = '#f06949';
			// ctx2.fill();


			// ctx2.save();

			// let showDots = () => {
			// 	for (let i = 0; i < 24; i++) {
			// 		ctx.beginPath();
			// 		let angle = Math.PI * 2 / 24 * i;
			// 		let translateX = canvasWidth * 0.5 + Math.cos(angle) * this.remUnit * 5.9;
			// 		let translateY = canvasHeight * 0.5 + Math.sin(angle) * this.remUnit * 5.9;
			// 		ctx.arc(translateX, translateY, this.remUnit * 0.35, this.remUnit, Math.PI * 2, true);
			// 		ctx.fillStyle = i % 2 === 0 ? this.dotsColorDictionary[0] : this.dotsColorDictionary[1];
			// 		ctx.fill();
			// 	}
			// };
			// showDots();
			// setInterval(() => {
			// 	showDots();
			// 	this.dotsColorDictionary = this.dotsColorDictionary.reverse();
			// }, 1000);

			let imageSequence = [];

			this.wheelData.forEach((item, index) => {
				let imageObj = new Image();
				imageObj.width = '150';
				imageObj.height = '150';
				// imageObj.src = this.$replaceProtocol(this.wheelData[index].image);
				imageObj.transparency = 0.2;
				imageSequence.push(imageObj);
			});


			this.wheelData.forEach((item, index) => {
				let angle = baseAngle * index;

				if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
					angle = angle - Math.PI;
				} else {
					angle = angle - Math.PI + baseAngle;
				}

				ctx.beginPath();
				ctx.moveTo(canvasWidth / 2, canvasHeight / 2);
				ctx.lineWidth = 3;
				if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
					ctx.arc(canvasWidth / 2, canvasHeight / 2, this.remUnit * 5.4, angle + baseAngle - Math.PI / this.wheelData.length, angle - Math.PI / this.wheelData.length, true);
				} else {
					ctx.arc(canvasWidth / 2, canvasHeight / 2, this.remUnit * 5.4, angle + baseAngle, angle, true);
				}
				ctx.lineTo(canvasWidth / 2, canvasHeight / 2);

				ctx.strokeStyle = '#4387BD';
				ctx.stroke();
				ctx.fillStyle = this.colorDictionary[index % 2];

				ctx.save();
				ctx.fill();

				// imageSequence[index].onload = () => {};

				setTimeout(() => {
					let translateX, translateY;
					if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
						translateX = canvasWidth * 0.5 + Math.cos(angle + baseAngle / 2 - Math.PI / 2 - Math.PI / this.wheelData.length) * this.remUnit * 5;
						translateY = canvasHeight * 0.5 + Math.sin(angle + baseAngle / 2 - Math.PI / 2 - Math.PI / this.wheelData.length) * this.remUnit * 5;
					} else {
						translateX = canvasWidth * 0.5 + Math.cos(angle + baseAngle / 2) * this.remUnit * 5;
						translateY = canvasHeight * 0.5 + Math.sin(angle + baseAngle / 2) * this.remUnit * 5;
					}

					ctx.font = this.remUnit * 0.6 + "px Georgia";
					ctx.fillStyle = this.textColorDictionary[index % 2];
					ctx.translate(translateX, translateY);
					ctx.rotate(angle);
					// ctx.fillText(this.wheelData[index].name, -ctx.measureText(this.wheelData[index].name).width / 2, 22);
					const offset = this.remUnit / 1.7;
					this.wheelData[index].name.split('').forEach((item, index) => {
						if (index < 6) {
							ctx.fillText(item, -ctx.measureText(item).width / 2, index * this.remUnit / 1.4 + offset);

						}
					});
					ctx.shadowColor = '#000'; // green for demo purposes
					ctx.shadowBlur = 10;
					ctx.shadowOffsetX = 0;
					ctx.shadowOffsetY = 0;


					ctx.restore();
					ctx.save();
				}, 500);




				// ctx.restore();
				// ctx.save();
				// let pointer = new Image();
				// pointer.url = 'http://localhost/static/img/pointer_00000.png';
				// pointer.width = '100';
				// pointer.height = '100';


			});
			setTimeout(() => {
				ctx.translate(200, 150);
				ctx.rotate(-Math.PI / 4);

				ctx.restore();
				ctx.save();

			}, 1000);

			this.getCachedCircleNumber();


		},
		drawPrize() {
			this.loading = true;
			// 本地调试代码
			console.log(Math.random());
			console.log(Math.ceil((this.wheelData.length - 1) * Math.random()));
			console.log(this.wheelData.find((item, index) => index === Math.ceil((this.wheelData.length - 1) * Math.random())));
			// let index = 0
			// this.wheelData.forEach((item1, index1) => {
			// 	if (index1 === Math.ceil((this.wheelData.length - 1) * Math.random())) {
			// 		index = index1
			// 		console.log('match+++++++', item1)
			// 		console.log('match+++++++', index)

			// 	}
			// })
			// if (!this.alreadyReleasedPrize) {
			// 	this.rotateWheel(index).then(() => {

			// 		this.alreadyReleasedPrize = true;

			// 		this.dailyLimit = Number(this.dailyLimit) > 0 ? Number(this.dailyLimit) - 1 : this.dailyLimit;

			// 	}).catch(error => {
			// 	})
			// }



			console.log(' this.userInfo.id', this.userInfo);
			this.$http.post(this.draw2Request, {
				u_id: this.userInfo.id
			}).then(response => {
				console.log(response);
				this.loading = false;
				response = response.data;

				if (response === null) {
					this.dialogRunOutFlag = true
				} else {
					this.prizeData = response;
					this.rewardCode = response.rewardCode;
					if (response.prize_Id) {
						this.wheelData.forEach((item1, index1) => {
							if (item1.value === response.prize_Id) {
								this.rotateWheel(index1).then(() => {
									this.alreadyReleasedPrize = true;
									if (response.id < 5) {
										this.dialogPrizeFlag = true;
									} else {
										this.dialogThankYouFlag = true;
									}
								});
							}
						});
					} else {
						if (response.prize_Id < 5) {
							this.dialogPrizeFlag = true;
						} else {
							this.dialogThankYouFlag = true;
						}
					}
				}


			}).catch(error => {
				this.loading = false;
				console.log(error);
			});
		},
		checkLowestCommonDivisorWith2(source) {
			let flag = true;
			for (let i = 2; i < source; i++) {
				source = source / 2;
				if (source !== 2) {
					flag = source % 2 === 0;
				}
			}
			return flag;
		},
		getCachedCircleNumber() {
			if (sessionStorage.getItem('actualRotate') !== undefined && sessionStorage.getItem('actualRotate') !== 0) {
				this.actualRotate = sessionStorage.getItem('actualRotate');
				let offsetAngle = this.actualRotate % 360;
				// alert(offsetAngle)
				// this.wheelCanvas.style.transform = 'rotate(' + this.actualRotate + 'deg)';
				this.wheelCanvas.style.transform = 'rotate(' + offsetAngle + 'deg)';
			}
		},
		rotateWheel(offset) {
			return new Promise((resolve, reject) => {
				console.log(111, offset);

				//因为canvas绘图顺序为顺时针，旋转顺序也为顺时针的话，旋转过后的个数会从最大值往最小值数，所以索性对偏移的个数进行取反

				offset = this.wheelData.length - offset - 4;
				let initRotateAngle = 3600;
				let unitAngle = 360 / this.wheelData.length;
				console.log(222, initRotateAngle + unitAngle * offset);
				this.actualRotate = initRotateAngle + unitAngle * offset;
				// alert(this.actualRotate)
				this.wheelCanvas.style.transform = 'rotate(-' + this.actualRotate + 'deg)';

				sessionStorage.setItem('actualRotate', this.actualRotate);
				if (!this.rotatingFlag) {
					this.rotatingFlag = true;
					this.wheelCanvas.style.transition = 'all ' + this.rotateDuration / 1000 + 's ease';
					this.wheelCanvas.style.transform = 'rotate(' + this.actualRotate + 'deg)';
					setTimeout(() => {
						this.rotatingFlag = false;
						this.wheelCanvas.style.transition = 'rotate 0s ease';
						resolve();
					}, this.rotateDuration);
				} else {
					reject();
				}
			});
		},

	}
}
</script>

<style scoped>
</style>
