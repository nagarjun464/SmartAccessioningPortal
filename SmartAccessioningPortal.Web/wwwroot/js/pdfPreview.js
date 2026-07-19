window.pdfPreview = {
    createUrl: function (inputId) {
        const input = document.getElementById(inputId);

        if (!input || !input.files || input.files.length === 0) {
            return null;
        }

        const file = input.files[0];

        return URL.createObjectURL(file);
    },

    revokeUrl: function (url) {
        if (url) {
            URL.revokeObjectURL(url);
        }
    }
};
