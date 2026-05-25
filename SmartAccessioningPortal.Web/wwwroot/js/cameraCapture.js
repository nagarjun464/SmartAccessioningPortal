window.cameraCapture = {
    stream: null,

    startCamera: async function (videoId) {
        const video = document.getElementById(videoId);

        if (!video) {
            return false;
        }

        this.stream = await navigator.mediaDevices.getUserMedia({
            video: true,
            audio: false
        });

        video.srcObject = this.stream;
        video.muted = true;
        video.playsInline = true;

        await new Promise((resolve) => {
            video.onloadedmetadata = () => {
                video.play();
                resolve();
            };
        });

        return true;
    },

    capturePhoto: async function (videoId, canvasId) {
        const video = document.getElementById(videoId);
        const canvas = document.getElementById(canvasId);

        if (!video || !canvas) {
            return null;
        }

        await new Promise(resolve => setTimeout(resolve, 500));

        if (video.videoWidth === 0 || video.videoHeight === 0) {
            return null;
        }

        const maxWidth = 640;
        const scale = maxWidth / video.videoWidth;

        canvas.width = maxWidth;
        canvas.height = video.videoHeight * scale;

        const context = canvas.getContext("2d");

        if (!context) {
            return null;
        }

        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        return canvas.toDataURL("image/jpeg", 0.6);
    },

    stopCamera: function () {
        if (this.stream) {
            this.stream.getTracks().forEach(track => track.stop());
            this.stream = null;
        }
    }
};