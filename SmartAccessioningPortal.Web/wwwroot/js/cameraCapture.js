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
        await video.play();

        return true;
    },

    capturePhoto: function (videoId, canvasId) {
        const video = document.getElementById(videoId);
        const canvas = document.getElementById(canvasId);

        if (!video || !canvas) {
            return null;
        }

        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        const context = canvas.getContext("2d");
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        return canvas.toDataURL("image/jpeg", 0.9);
    },

    stopCamera: function () {
        if (this.stream) {
            this.stream.getTracks().forEach(track => track.stop());
            this.stream = null;
        }
    }
};