
window.logger = {
    info: function (message) {
        console.info(`%cINFO: %c${message}`, 'color: blue; font-weight: bold;', 'color: white;');
    },
    warn: function (message) {
        console.warn(`%cWARN: %c${message}`, 'color: orange; font-weight: bold;', 'color: white;');
    },
    error: function (message) {
        console.error(`%cERROR: %c${message}`, 'color: red; font-weight: bold;', 'color: white;');
    },
    debug: function (message) {
        console.debug(`%cDEBUG: %c${message}`, 'color: green; font-weight: bold;', 'color: white;');
    },
    object: function (obj) {
        console.log('%cOBJECT:', 'color: purple; font-weight: bold;', obj);
    }
};

