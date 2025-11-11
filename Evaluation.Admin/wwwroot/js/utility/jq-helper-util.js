const commonUtil = (function (options) {

    const dailogId = 'files-dailog';
    const contentDailogId = 'content-dailog';
    const btnCloseDailogId = "btn_dialog_titlebar_close";
    const EVT_DAILOG_CLOSE = "DAILOG_CLOSE";
    const viewDetailsContainerId = "view-details-container";
    const closeFullPopupId = 'close-view-details-container';
    const appContentModalBodyId ='appContentModalBody';
    const appContentModalId ='appContentModal';

    const MIME_TYPES =
    {
        'a': 'application/octet-stream',
        'ai': 'application/postscript',
        'aif': 'audio/x-aiff',
        'aifc': 'audio/x-aiff',
        'aiff': 'audio/x-aiff',
        'au': 'audio/basic',
        'avi': 'video/x-msvideo',
        'aac': "audio/aac",
        'bat': 'text/plain',
        'bin': 'application/octet-stream',
        'bmp': 'image/x-ms-bmp',
        'c': 'text/plain',
        'cdf': 'application/x-cdf',
        'csh': 'application/x-csh',
        'css': 'text/css',
        'dll': 'application/octet-stream',
        'doc': '.doc',
        'docx': '.docx',
        //'doc': 'application/msword',
        //'docx': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        'dot': 'application/msword',
        'dvi': 'application/x-dvi',
        'eml': 'message/rfc822',
        'eps': 'application/postscript',
        'etx': 'text/x-setext',
        'exe': 'application/octet-stream',
        'gif': 'image/gif',
        'gtar': 'application/x-gtar',
        'h': 'text/plain',
        'hdf': 'application/x-hdf',
        'htm': 'text/html',
        'html': 'text/html',
        'jif': 'image/jif',
        'jfif': 'image/jfif',
        'jfi': 'image/jfi',
        'jpe': 'image/jpeg',
        'jpeg': 'image/jpeg',
        'jpg': 'image/jpeg',
        'js': 'application/x-javascript',
        'ksh': 'text/plain',
        'latex': 'application/x-latex',
        'm1v': 'video/mpeg',
        'man': 'application/x-troff-man',
        'me': 'application/x-troff-me',
        'mht': 'message/rfc822',
        'mhtml': 'message/rfc822',
        'mif': 'application/x-mif',
        'mov': 'video/quicktime',
        'mpv': 'video/x-project',
        'movie': 'video/x-sgi-movie',
        'mp2': 'audio/mpeg',
        'mp3': 'audio/mpeg',
        'mp4': 'video/mp4',
        'mpa': 'video/mpeg',
        'mpe': 'video/mpeg',
        'mpeg': 'video/mpeg',
        'mpg': 'video/mpeg',
        'webm': 'video/webm',
        'm4p': 'video/m4p',
        'm4v': 'video/m4v',
        'wmv': 'video/wmv',
        'flv': 'video/x-flv',
        'f4v': 'video/x-f4v',
        'AVCHD': 'video/mts',
        'ms': 'application/x-troff-ms',
        'nc': 'application/x-netcdf',
        'nws': 'message/rfc822',
        'o': 'application/octet-stream',
        'obj': 'application/octet-stream',
        'oda': 'application/oda',
        "oga": "audio/ogg",
        "ogv": "video/ogg",
        "ogg": "video/ogg",
        'pbm': 'image/x-portable-bitmap',
        'pdf': 'application/pdf',
        'pfx': 'application/x-pkcs12',
        'pgm': 'image/x-portable-graymap',
        'png': 'image/png',
        'pnm': 'image/x-portable-anymap',
        'pot': 'application/vnd.ms-powerpoint',
        'ppa': 'application/vnd.ms-powerpoint',
        'ppm': 'image/x-portable-pixmap',
        'pps': 'application/vnd.ms-powerpoint',
        'ppt': 'application/vnd.ms-powerpoint',
        'pptx': 'application/vnd.ms-powerpoint',
        'ps': 'application/postscript',
        'pwz': 'application/vnd.ms-powerpoint',
        'py': 'text/x-python',
        'pyc': 'application/x-python-code',
        'pyo': 'application/x-python-code',
        'qt': 'video/quicktime',
        'ra': 'audio/x-pn-realaudio',
        'ram': 'application/x-pn-realaudio',
        'ras': 'image/x-cmu-raster',
        'rdf': 'application/xml',
        'rgb': 'image/x-rgb',
        'roff': 'application/x-troff',
        'rtx': 'text/richtext',
        'sgm': 'text/x-sgml',
        'sgml': 'text/x-sgml',
        'sh': 'application/x-sh',
        'shar': 'application/x-shar',
        'snd': 'audio/basic',
        'so': 'application/octet-stream',
        'src': 'application/x-wais-source',
        'swf': 'application/x-shockwave-flash',
        "svg": "image/svg+xml",
        't': 'application/x-troff',
        'tar': 'application/x-tar',
        'tcl': 'application/x-tcl',
        'tex': 'application/x-tex',
        'texi': 'application/x-texinfo',
        'texinfo': 'application/x-texinfo',
        'tif': 'image/tiff',
        'tiff': 'image/tiff',
        'tr': 'application/x-troff',
        'tsv': 'text/tab-separated-values',
        'txt': 'text/plain',
        'ustar': 'application/x-ustar',
        'vcf': 'text/x-vcard',
        'wav': 'audio/x-wav',
        'wiz': 'application/msword',
        'wsdl': 'application/xml',
        'xbm': 'image/x-xbitmap',
        'xlb': 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'xls': 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'xlsx': 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'xml': 'text/xml',
        'xpdl': 'application/xml',
        'xpm': 'image/x-xpixmap',
        'xsl': 'application/xml',
        'xwd': 'image/x-xwindowdump',
        'zip': 'application/zip',
        "3gp": "video/3gpp",
    };

    const CONTAINER_TYPE = {
        COMPLAINTS: "COMPLAINTS"
        
    };

    const STATUS_MASTER_TYPE = {
        SUBMIT: "SUBMIT",
        UNASSIGN: "UNASSIGN",
        INPROGRESS: "INPROGRESS",
        ASSIGNED: "ASSIGNED",
        REJECT: "REJECT",
        RETURN_BACK_TO_OWNER: "RETURN_BACK_TO_OWNER",
        ASK_MORE_INFO: "ASK_MORE_INFO",
        RESUBMIT: "RESUBMIT",
        RESUBMIT_BY_OWNER: "RESUBMIT_BY_OWNER",
        WAITING_FOR_RESPONSE: "WAITING_FOR_RESPONSE",
        SENT_TO_FEEDBACK: "SENT_TO_FEEDBACK",
        FEEDBACK_COMPLETED: "FEEDBACK_COMPLETED",
        FEEDBACK_SUBMITED: "FEEDBACK_SUBMITED",
        RETURN_TO_FEEDBACK: "RETURN_TO_FEEDBACK",
        ASK_MORE_FEEDBACK: "ASK_MORE_FEEDBACK",
        FEEDBACK_RESUBMITED: "FEEDBACK_RESUBMITED",
        READY_TO_AGENDA: "READY_TO_AGENDA",
        AGENDA_ASSIGNED: "AGENDA_ASSIGNED",
        WAITING_FOR_AGENDA_REPORT: "WAITING_FOR_AGENDA_REPORT",
        AGENDA_REPORT_COMPLETED: "AGENDA_REPORT_COMPLETED",
        AGENDA_REPORT_SUBMIT: "AGENDA_REPORT_SUBMIT",
        AGENDA_REPORT_APPROVED_DEP: "AGENDA_REPORT_APPROVED_DEP",
        AGENDA_REPORT_APPROVED_AGENDA: "AGENDA_REPORT_APPROVED_AGENDA",
        AGENDA_RESCHEDULE: "AGENDA_RESCHEDULE",
        CLOSED: "CLOSED",
        CLOSED_DEP: "CLOSED_DEP",
        EDIT_OWNER: "EDIT_OWNER",
        DELETE_OWNER: "DELETE_OWNER",
        EDIT_DEP: "EDIT_DEP",
        DELETE_DEP: "DELETE_DEP",
        //extra   status keys
        CREATE_AGENDA: "CREATE_AGENDA",
        EDIT_AGENDA: "EDIT_AGENDA",
        ASSIGN_COMPS_FROM_AGENDA: "ASSIGN_COMPS_FROM_AGENDA",
        REMOVE_COMPLAINT_AGENDA: "REMOVE_COMPLAINT_AGENDA",
        NOT_DISCUSS: "NOT_DISCUSS",
        DISCUSSED: "DISCUSSED",
        AGENDA_DELETE: "AGENDA_DELETE",
        AGENDA_REMINDER: "AGENDA_REMINDER",
        ADD_ATTACHMENT: "ADD_ATTACHMENT",
    };

    const PERMISSIONS_KEYS = {
        SUBMIT:"SUBMIT",
        VIEWMYCOMPLAINT:"ViewMyComplaint",
        VIEWASSIGNEDCOMPLAINTS:"ViewAssignedComplaints",
        VIEWMANAGERCOMPLAINTS:"ViewManagerComplaints",
        EDIT_OWNER:"EDIT_OWNER",
        DELETE_OWNER:"DELETE_OWNER",
        DeleteComplaintFile:"DeleteComplaintFile",
        SHOW_FEEDBACK_TAB:"SHOW_FEEDBACK_TAB",
        VIEW_BTN_ADMIN_NAVBAR:"VIEW_BTN_ADMIN_NAVBAR",
        ASSIGNED:"ASSIGNED",
        REJECT:"REJECT",
        RETURN_BACK_TO_OWNER:"RETURN_BACK_TO_OWNER",
        RESUBMIT:"RESUBMIT",
        SENT_TO_FEEDBACK:"SENT_TO_FEEDBACK",
        FEEDBACK_SUBMITED:"FEEDBACK_SUBMITED",
        RETURN_TO_FEEDBACK:"RETURN_TO_FEEDBACK",
        FEEDBACK_RESUBMITED:"FEEDBACK_RESUBMITED",
        READY_TO_AGENDA:"READY_TO_AGENDA",
        AGENDA_REPORT_SUBMIT:"AGENDA_REPORT_SUBMIT",
        AGENDA_REPORT_APPROVED_AGENDA:"AGENDA_REPORT_APPROVED_AGENDA",
        CLOSED_DEP:"CLOSED_DEP",
        ADD_NOTE:"ADD_NOTE",
        CREATE_AGENDA:"CREATE_AGENDA",
        EDIT_AGENDA:"EDIT_AGENDA",
        REMOVE_COMPLAINT_AGENDA:"REMOVE_COMPLAINT_AGENDA",
        VIEW_PERMISSIONS:"VIEW_PERMISSIONS",
        VIEWUSERPERMISSIONS:"VIEWUSERPERMISSIONS",
        ViewApplicationSettings:"ViewApplicationSettings",
        VIEW_AGENDA:"VIEW_AGENDA",
        VIEW_COMPLAINTS_IN_AGENDA:"VIEW_COMPLAINTS_IN_AGENDA",
        ASSIGN_COMPLAINT_TO_AGENDA: "ASSIGN_COMPLAINT_TO_AGENDA",
        Agenda_Reminder:"Agenda_Reminder",
        
        REPORT_COMPLAINT_DETAILS:"REPORT_COMPLAINT_DETAILS",
        FeedbackReminder:"FeedbackReminder",
    }

    const STATUS_TYPE = {
        OWNER: "OWNER",
        DEPARTMENT: "DEPARTMENT",
        FEEDBACK: "FEEDBACK",
        AGENDA: "AGENDA",
    };

    const FILES_TYPE = {
        IMAGE: `<i class="fa fa-image"></i>`.trim(),
        VIDEO: `<i class="fa fa-camera-video"></i>`.trim(),
        MP3: `<i class="fa fa-file-audio"></i>`.trim(),
        PDF: `<i class="fa fa-file-pdf"></i>`.trim(),
        WORD: `<i class="fa fa-file-word"></i>`.trim(),
        EXCEL: `<i class="fa fa-file-excel"></i>`.trim(),
        TEXT: `<i class="fa fa-file-text"></i>`.trim(),
        OTHER: `<i class="fa fa-file"></i>`.trim()
    };

    const DATE_FORMAT = {
        SLASH_M_D_Y: "MM/DD/YYYY",
        DASH_YYYY_MM_DD: "YYYY-MM-DD",
        DASH_YYYY_MM_DDLT: "YYYY-MM-DDLT",
        DASH_YYYY_MM_DDTHH_mm_ss: "YYYY-MM-DDTHH:mm:ss",
        DASH_YYYY_MM_DD_HH_mm_ss: "YYYY-MM-DD HH:mm:ss",
        SLASH_D_M_Y_DDTHH_mm: "DD/MM/YYYYTHH:mm",
        SLASH_M_D_Y_DDTHH_mm_ss: "MM/DD/YYYYTHH:mm:ss",
        lll: "lll",
        ll: "ll",
        LT: "LT",
        LL: 'LL'
    };

    const uuidv4 = () => {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    const getLocalUtcDateString = (utcDateString, timezoneId, locale, formate) => {
        if (!utcDateString) return '';
        moment.locale(locale);
        var utcMoment = moment.utc(utcDateString, formate ?? 'MM/DD/YYYY hh:mm:ss A');
        var localTimeTz = moment.utc(utcMoment).tz(timezoneId).format(formate ?? 'MM/DD/YYYY hh:mm:ss A');
        return localTimeTz;
    };

    const getLocalUtcDateStringEn = (utcDateString, format) => {
        if (!utcDateString) return '';
        moment.locale("en");
        format = format ?? 'MM/DD/YYYY hh:mm:ss A';
        var utcMoment = moment.utc(new Date(utcDateString), 'MM/DD/YYYY hh:mm:ss A');
        var localTimeTz = moment.utc(utcMoment).tz("Asia/Riyadh").format(format);
        return localTimeTz;
    };

    const getLocalUtcDateStringAr = (utcDateString, format) => {
        if (!utcDateString) return '';
        moment.locale("ar");
        format = format ?? 'MM/DD/YYYY hh:mm:ss A';
        var utcMoment = moment.utc(new Date(utcDateString), 'MM/DD/YYYY hh:mm:ss A');
        var localTimeTz = moment.utc(utcMoment).tz("Asia/Riyadh").format(format);
        return localTimeTz;
    };

    const getLocalUtcDateEn = (utcDateString) => {
        if (!utcDateString) return '';
        moment.locale("en");
        var utcMoment = moment.utc(new Date(utcDateString), 'MM/DD/YYYY');
        var localTimeTz = moment.utc(utcMoment).tz("Asia/Riyadh").format('MM/DD/YYYY');
        return localTimeTz;
    };

    const getLocalUtcDateAr = (utcDateString) => {
        if (!utcDateString) return '';
        moment.locale("ar");
        var utcMoment = moment.utc(new Date(utcDateString), 'MM/DD/YYYY');
        var localTimeTz = moment.utc(utcMoment).tz("Asia/Riyadh").format('MM/DD/YYYY');
        return localTimeTz;
    };

    const getMonthStartDateNEndDate = (date) => {
        moment.locale("en");
        return {
            startOfMonth: moment(date).startOf('month').format(commonUtil.DATE_FORMAT.SLASH_M_D_Y),
            endOfMonth: moment(date).endOf('month').format(commonUtil.DATE_FORMAT.SLASH_M_D_Y)
        };
    };

    const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const getAllUrlParams = (url) => {

        // get query string from url (optional) or window
        var queryString = url ? url.split('?')[1] : window.location.search.slice(1);

        // we'll store the parameters here
        var obj = {};

        // if query string exists
        if (queryString) {

            // stuff after # is not part of query string, so get rid of it
            queryString = queryString.split('#')[0];

            // split our query string into its component parts
            var arr = queryString.split('&');

            for (var i = 0; i < arr.length; i++) {
                // separate the keys and the values
                var a = arr[i].split('=');

                // set parameter name and value (use 'true' if empty)
                var paramName = a[0];
                var paramValue = typeof (a[1]) === 'undefined' ? true : a[1];

                // (optional) keep case consistent
                paramName = paramName.toLowerCase();
                if (typeof paramValue === 'string') paramValue = paramValue.toLowerCase();

                // if the paramName ends with square brackets, e.g. colors[] or colors[2]
                if (paramName.match(/\[(\d+)?\]$/)) {

                    // create key if it doesn't exist
                    var key = paramName.replace(/\[(\d+)?\]/, '');
                    if (!obj[key]) obj[key] = [];

                    // if it's an indexed array e.g. colors[2]
                    if (paramName.match(/\[\d+\]$/)) {
                        // get the index value and add the entry at the appropriate position
                        var index = /\[(\d+)\]/.exec(paramName)[1];
                        obj[key][index] = paramValue;
                    } else {
                        // otherwise add the value to the end of the array
                        obj[key].push(paramValue);
                    }
                } else {
                    // we're dealing with a string
                    if (!obj[paramName]) {
                        // if it doesn't exist, create property
                        obj[paramName] = paramValue;
                    } else if (obj[paramName] && typeof obj[paramName] === 'string') {
                        // if property does exist and it's a string, convert it to an array
                        obj[paramName] = [obj[paramName]];
                        obj[paramName].push(paramValue);
                    } else {
                        // otherwise add the property
                        obj[paramName].push(paramValue);
                    }
                }
            }
        }

        return obj;
    }

    const getTextDirection = () => {
        const val = $('#lbl_lang').text();
        return (val && val.trim().toUpperCase()) === 'EN' ? "RTL" : "LTR";
    };

    const downloadBase64File = (contentBase64, fileName, contentType) => {
        const linkSource = `data:${contentType};base64,${contentBase64}`;
        const downloadLink = document.createElement('a');
        document.body.appendChild(downloadLink);

        downloadLink.href = linkSource;
        downloadLink.target = '_self';
        downloadLink.download = fileName;
        downloadLink.click();
    };

    const removeEntities = (str) => {
        return ('' + str).replace(/&\w+;\s*/g, '');
    };

    const isRTL = (value) => {
        if (!value) return false;
        const type = whatIsIt(value);
        if (type === "NULL") return false;
        if (type === "UNDEFINED") return false;
        if (type !== "STRING") return false;
        if (type === "ARRAY") return false;
        if (type === "OBJECT") return false;

        var regex = /(<([^>]+)>)/ig
            , result = value.replace(regex, "");
        result = removeEntities(result);
        var ltrChars = 'A-Za-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02B8\u0300-\u0590\u0800-\u1FFF' + '\u2C00-\uFB1C\uFDFE-\uFE6F\uFEFD-\uFFFF',
            rtlChars = '\u0591-\u07FF\uFB1D-\uFDFD\uFE70-\uFEFC',
            rtlDirCheck = new RegExp('^[^' + ltrChars + ']*[' + rtlChars + ']');
        return rtlDirCheck.test(result);
    };

    const getImagesFormat = () => {
        return `jpg, jpeg, jpe, jif, jfif, jfi, png, gif, webp, tiff, tif,
                            psd, raw, arw, cr2, nrw, k25,
                            bmp, dib,
                            heif, heic,
                            ind, indd, indt,
                            jp2, j2k, jpf, jpx, jpm, mj2,
                            svg, svgz
                            `.replace(/\s/g, '').split(',');
    };

    const getFileIcon = (filename) => {
        const images = `jpg, jpeg, jpe, jif, jfif, jfi, png, gif, webp, tiff, tif,
                            psd, raw, arw, cr2, nrw, k25,
                            bmp, dib,
                            heif, heic,
                            ind, indd, indt,
                            jp2, j2k, jpf, jpx, jpm, mj2,
                            svg, svgz
                            `.replace(/\s/g, '').split(',');
        const pdf = `eps, pdf, ai`.replace(/\s/g, '').split(',');
        const video = `webm, mpg, mp2, mp3, mpeg, mpe, mpv, ogg, mp4, m4p, 
                            m4v, avi, wmv, mov, qt, flv, swf, AVCHD`.replace(/\s/g, '').split(',');
        const word = `doc, docm, docx, dot, dotm, dotx, rtf, pptx, ppt, pptm`.replace(/\s/g, '').split(',');
        const excel = `csv, xlsx, xla, xlam, xls, xlsb, xlsm, xlsx, xlt, xltm`.replace(/\s/g, '').split(',');
        const type = filename.split('.').pop().toLowerCase();
        let icon = '';
        if (images.includes(type)) {
            icon = 'fa-image';
        } else if (pdf.includes(type)) {
            icon = 'fa-file-pdf';
        } else if (video.includes(type)) {
            icon = 'fa-camera-video';
        } else if (word.includes(type)) {
            icon = 'fa-file-word';
        } else if (excel.includes(type)) {
            icon = 'fa-file-excel';
        } else if ('txt' == type) {
            icon = 'fa-file-text';
        } else {
            icon = 'fa-file';
        }
        return icon = `<i class="fa ${icon}"></i>`;
    }

    const getFileIconClazz = (filename) => {
        if (!filename) return;

        const images = `jpg, jpeg, jpe, jif, jfif, jfi, png, gif, webp, tiff, tif,
                            psd, raw, arw, cr2, nrw, k25,
                            bmp, dib,
                            heif, heic,
                            ind, indd, indt,
                            jp2, j2k, jpf, jpx, jpm, mj2,
                            svg, svgz
                            `.replace(/\s/g, '').split(',');
        const pdf = `eps, pdf, ai`.replace(/\s/g, '').split(',');
        const video = `webm, mpg, mp2, mp3, mpeg, mpe, mpv, ogg, mp4, m4p, 
                            m4v, avi, wmv, mov, qt, flv, swf, AVCHD`.replace(/\s/g, '').split(',');
        const word = `doc, docm, docx, dot, dotm, dotx, rtf, pptx, ppt, pptm`.replace(/\s/g, '').split(',');
        const excel = `csv, xlsx, xla, xlam, xls, xlsb, xlsm, xlsx, xlt, xltm`.replace(/\s/g, '').split(',');
        const type = filename.split('.').pop().toLowerCase();
        let icon = '';
        if (images.includes(type)) {
            icon = 'fa-image';
        } else if (pdf.includes(type)) {
            icon = 'fa-file-pdf';
        } else if (video.includes(type)) {
            icon = 'fa-camera-video';
        } else if (word.includes(type)) {
            icon = 'fa-file-word';
        } else if (excel.includes(type)) {
            icon = 'fa-file-excel';
        } else if ('txt' == type) {
            icon = 'fa-file-text';
        } else {
            icon = 'fa-file';
        }
        return icon = `fa ${icon}`;
    };

    const toMMDDYYYY = (value, formate = DATE_FORMAT.SLASH_M_D_Y, locale) => {
        moment.locale(locale ?? "en");
        return moment(new Date(value), 'DD-MM-YYYY').format(formate);
    };

    const toFormatedDate = (value, inputFormat, outFormate) => {
        moment.locale("en");
        const momentObj = moment(new Date(value), inputFormat || DATE_FORMAT.SLASH_M_D_Y)
        const date = momentObj.format(outFormate || DATE_FORMAT.DASH_YYYY_MM_DD);
        return {
            date,
            momentObj
        };
    };

    const concatDateNTime = (date, time, inFormate, outFormate = DATE_FORMAT.DASH_YYYY_MM_DDTHH_mm_ss) => {
        moment.locale("en");
        const momentObj = moment(date + time, DATE_FORMAT.DASH_YYYY_MM_DDLT);
        return momentObj.format(outFormate);
    };

    const toDateTime = (value, format = DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss) => {
        moment.locale("en");
        return moment(new Date(value)).format(format);
    };

    const addDayToDate = (value, days, format = DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss) => {
        moment.locale("en");
        //var start = moment(contractMoment);
        return moment(new Date(value)).add(days, 'days').format(format);
    };

    const toFormateMoment = (value, format = DATE_FORMAT.DASH_YYYY_MM_DD) => {
        moment.locale("en");
        return moment(value).format(format);
    };

    const getOnlyTime = (value, format = DATE_FORMAT.DASH_YYYY_MM_DD) => {
        moment.locale("en");
        return moment(value).format("hh:mm:ss a");
    };

    

    const IsJsonString = (str) => {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }

    const isValidEmail = value => {
        const pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return value.match(pattern);

    }

    const createProgressBar = ({ maxVal, minVal, isComplete, appendId, step, delay, isHideAfterMaxVal, timePeriod }) => {
        minVal = minVal ?? 20;
        maxVal = maxVal ?? 90;
        step = step ?? 5;
        delay = delay ?? 500;
        isHideAfterMaxVal = isHideAfterMaxVal ?? false;


        isComplete = isComplete || false;
        const progressId = `dynamic-progress-bar-${appendId || ''}`;
        const progressDivId = `dynamic-progress-bar-div-${appendId || ''}`;
        let $progressBarElem = $('#' + progressId);

        const markup = `
                        <div class="progress" id="${progressDivId}">
                          <div id="${progressId}" class="progress-bar progress-bar-success progress-bar-striped progress-bar-animated active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                            <span id="current-progress"></span>
                          </div>
                        </div>
                        `;

        if (timePeriod && timePeriod > 0) {
            minVal = 0;
            maxVal = 1000 * timePeriod;
            if (appendId) {
                $('#' + appendId).append(markup);
            }
            //maxVal = (timePeriod * 1000) / 1;
            let progress = minVal;
            let interval = setInterval(function () {
                progress += 1000;
                const progressVal = Math.floor((progress / maxVal) * 100);
                $("#" + progressId)
                    .css("width", progressVal + "%")
                    .attr("aria-valuenow", progressVal)
                    .text(progressVal + "% Complete");
                if (progress >= maxVal) {
                    clearInterval(interval);
                    if (isHideAfterMaxVal) {
                        setTimeout(() => { $('#' + progressDivId).remove(); }, 500);
                        return;
                    }
                }
            }, 1000);
            return markup;
        }

        if ($progressBarElem && $progressBarElem.length && isComplete) {
            const finalVal = 100;
            $progressBarElem
                .css("width", finalVal + "%")
                .attr("aria-valuenow", finalVal)
                .text(finalVal + "% Complete");
            setTimeout(() => { $('#' + progressDivId).remove() }, 500);
        }
        else if ($progressBarElem && $progressBarElem.length) {
        }
        else {

            if (appendId) {
                $('#' + appendId).append(markup);
            }
            let progress = minVal;
            let interval = setInterval(function () {
                progress += step;
                $("#" + progressId)
                    .css("width", progress + "%")
                    .attr("aria-valuenow", progress)
                    .text(progress + "% Complete");
                if (progress >= maxVal) {
                    clearInterval(interval);
                    if (isHideAfterMaxVal) {
                        setTimeout(() => { $('#' + progressDivId).remove(); }, 500);
                        return;
                    }
                }
            }, delay);

            return markup;
        }
    };

    const createLoader = (appendId, isComplete = false, isClearAppendSection) => {
        if (isClearAppendSection) {
            $('#' + appendId).empty();
            return;
        }
        const id = "progress-loader";
        let $progressElem = $('#' + id);

        if ($progressElem && $progressElem.length && isComplete) {
            setTimeout(() => {
                $('#' + appendId).empty();
            }, 10);
        } else if ($progressElem && $progressElem.length && !isComplete) {
            setTimeout(() => {
                $('#' + appendId).empty();
                //$progressElem.remove();
            }, 10);

        } else if ($progressElem && $progressElem.length) {

        } else {
            const loaderElem = $('<img/>').attr({
                id: id,
                //src: '/images/HopeWell.gif',
                src: '/img/special-spinner.gif',
                width: '70px',
                height: '70px'
            });
            $('#' + appendId).append(loaderElem);
        }
    };

    const createSectionLoader = ({ appendId, isComplete, width, height, isForceFullEmpty }) => {
        const imgId = 'section-' + appendId;
        width = width || '144px';
        height = height || '144px';
        let $progressElem = $('#' + imgId);
        if (isForceFullEmpty) {
            if ($progressElem && $progressElem.length) {
                $progressElem.remove();
            }
            return;
        }

        if ($progressElem && $progressElem.length && isComplete) {

        } else if ($progressElem && $progressElem.length && !isComplete) {
            $progressElem.remove();
        } else if ($progressElem && $progressElem.length) {

        } else {
            const loaderElem = $('<img/>').attr({
                id: imgId,
                src: '/images/myloader.gif',
                width: width,
                height: height
            });
            $('#' + appendId).append(loaderElem);
        }
    };

    const stringConstructor = "test".constructor;
    const arrayConstructor = [].constructor;
    const objectConstructor = ({}).constructor;

    const whatIsIt = (object) => {
        if (object === null) {
            return "NULL";
        }
        if (object === undefined) {
            return "UNDEFINED";
        }
        if (object.constructor === stringConstructor) {
            return "STRING";
        }
        if (object.constructor === arrayConstructor) {
            return "ARRAY";
        }
        if (object.constructor === objectConstructor) {
            return "OBJECT";
        }
        {
            return "DONT_KNOW";
        }
    };

    const getDataType = (data) => {
        return whatIsIt(data);
    };

    const serverError = (XMLHttpRequest) => {
        const type = whatIsIt(XMLHttpRequest);
        if (type === "NULL") {
            return notificationUtil.error("Technical Error..!!!");
        }
        if (type === "UNDEFINED") {
            return notificationUtil.error("Technical Error..!!!");
        }
        if (type === "STRING") {
            return notificationUtil.error(XMLHttpRequest);;
        }
        if (type === "ARRAY") {
            return "ARRAY";
        }
        if (type === "OBJECT") {
            const { responseText, responseJSON } = XMLHttpRequest;
            if (responseJSON) {
                const { message } = responseJSON;
                return notificationUtil.error(message);
            }
            else if (responseText) {
                return notificationUtil.error(responseText.substring(0, 100));
            }
            else if (XMLHttpRequest.statusCode == 400) {
                location.reload(true);

            }
            else if (XMLHttpRequest.statusCode == 0) {
                location.reload();
            }
        }


        if (XMLHttpRequest && !commonUtil.isJsonString(XMLHttpRequest) && !XMLHttpRequest.hasOwnProperty('responseJSON')) {
            notificationUtil.error(XMLHttpRequest.message);
            return;
        }
        const { responseText, responseJSON } = XMLHttpRequest;
        if (responseJSON) {
            const { message } = responseJSON;
            notificationUtil.error(message);
        }
        else if (responseText && commonUtil.isJsonString(responseText)) {
            const { status, errors, message } = JSON.parse(responseText);
            if (errors) {
                let ul = $('<ul/>');
                errors.forEach(err => {
                    ul.append($('<li/>').html(err));
                });
                alert(ul);
            }
            else if (message) {
                try {
                    notificationUtil.error(message);
                } catch (e) {
                    notificationUtil.error(responseText);
                }
            }
        } else {
            notificationUtil.error(responseText);
        }
    };


    

    const activateAdminTab = () => {
        const url = window.location.href;
        const regex = /(?<=admin).+/;
        const match = url.match(regex);
        if (!match || !match[0]) {
            const elem = document.querySelector(`[li-key="li-dashboard"]`);
            elem.parentElement.classList.add('active');
            return;
        }

        let tabId = storeUtil.getStore(storeUtil.STORE_KEY.TAB_ID);
        if (!tabId) {
            tabId = 'li-' + match[0].substring(1);
            if (tabId) {
                storeUtil.setStore(storeUtil.STORE_KEY.TAB_ID, tabId);
            }
        }
        const elem = document.querySelector(`[li-key="${tabId.toLowerCase()}"]`);
        if (elem) {
            elem.parentElement.classList.add('active');
        }
    }

    const stringToBoolean = val => {
        if (!val) return false;

        switch (val.toLowerCase().trim()) {
            case "true": case "yes": case "1": return true;
            case "false": case "no": case "0": case null: return false;
            default: return Boolean(val);
        }
    }

    const showThumbnail01 = (blobName, fileName, storageContainerType, appendId) => {
        const $append = $('#' + appendId);
        if (blobName) {
            $append.empty();
            const icon = commonUtil.getFileIcon(blobName);
            if (icon.trim() === FILES_TYPE.IMAGE) {
                createLoader(appendId, true);
                jqClient.GetBase64(
                    { blobName: blobName, storageContainerType },
                    (result) => {
                        $append.empty();
                        if (result) {
                            const { data } = result;
                            if (data) {
                                const { base64File, contentType, url } = data;
                                const imgSrc = (base64File && contentType)
                                    ? `data:${contentType || ''};base64,${base64File || ''}`
                                    : (url ?? '');
                                const img = $('<img />').attr({ 'src': imgSrc, id: 'thumbnailImg' })
                                    .addClass('img-fluid form-control img-size');
                                $append.empty().append(img);
                            }
                        }
                    }
                )
            } else {
                const downThumId = 'downloadThumbnailId';
                const attchment = $('<span />').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                $append.append(attchment);
                $("#" + downThumId).click(function () {
                    jqClient.DownloadFile({ blobName, storageContainerType, fileName, loaderId: appendId });
                });
            }
        }
    }

    const showThumbnail = (blobName, fileName, storageContainerType, appendId) => {
        const $append = $('#' + appendId);
        if (blobName) {
            $append.empty();
            const icon = commonUtil.getFileIcon(blobName);
            if (icon.trim() === FILES_TYPE.IMAGE) {
                createLoader(appendId, true);
                jqClient.GetFileUrl(
                    { blobName: blobName, storageContainerType, fileName },
                    (result) => {
                        $append.empty();
                        if (result) {
                            const { data } = result;
                            if (data) {
                                const { base64File, contentType, url } = data;
                                const imgSrc = (base64File && contentType)
                                    ? `data:${contentType || ''};base64,${base64File || ''}`
                                    : (url ?? '');
                                const img = $('<img />').attr({ 'src': imgSrc, id: 'thumbnailImg' })
                                    .addClass('img-fluid form-control img-size');
                                $append.empty().append(img);
                            }
                        }
                    }
                )
            } else {
                const downThumId = 'downloadThumbnailId';
                const attchment = $('<span />').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                $append.append(attchment);
                $("#" + downThumId).click(function () {
                    jqClient.DownloadFile({ blobName, storageContainerType, fileName, loaderId: appendId });
                });
            }
        }
    }

    const showThumbnailFromAppFolder = (blobName, appendId, folderPath) => {
        const $append = $('#' + appendId);
        if (blobName) {
            $append.empty();
            const icon = commonUtil.getFileIcon(blobName);
            if (icon.trim() === FILES_TYPE.IMAGE) {
                //const src = `/images/icon/${blobName}`;
                const src = `${folderPath}/${blobName}`
                const img = $('<img />').attr({ 'src': src, id: 'thumbnailImg' }).addClass('img-fluid img-size');
                $append.append(img);
            }
        }
    }

    const blobToBase64 = blob => {
        const reader = new FileReader();
        reader.readAsDataURL(blob);
        return new Promise(resolve => {
            reader.onloadend = () => {
                resolve(reader.result);
            };
        });
    };

    const base64toBlob = (b64Data, contentType = '', sliceSize = 512) => {
        const byteCharacters = atob(b64Data);
        const byteArrays = [];

        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);

            const byteNumbers = new Array(slice.length);
            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }

        const blob = new Blob(byteArrays, { type: contentType });
        return blob;
    };

    const createDailog = ({ dailogId, title, eventBtn, isShowCancle }) => {
        let buttonArr = [];
        if (isShowCancle) {
            const btnCancle = {
                id: "dailog-btn-close",
                text: "Close",
                class: 'btn-primary',
                //icons: { primary:"ui-icon-closethick"},
                click: function () {
                    $(`#${dailogId}`).empty();
                    $(this).dialog("close");
                }
            };
            buttonArr.push(btnCancle);
        }
        if (eventBtn && !_.isEmpty(eventBtn)) {
            const { btnId, btnLabel, data, eventName } = eventBtn;
            const evtBtn = {
                id: btnId,
                text: btnLabel,
                class: 'btn btn-primary',
                //icons: { primary: "ui-icon-arrowthickstop-1-s" },
                click: function (e) {
                    const evElem = document.getElementById(btnId);
                    const event = new CustomEvent(eventName, { detail: { type: `type__${eventName}`, data: data } });
                    evElem.dispatchEvent(event);
                }
            };
            buttonArr.unshift(evtBtn);
        }
        const dialogElem = $("#" + dailogId).dialog({
            title: title || '',
            autoOpen: false,
            width: '90vw',
            //height: '90vh',
            modal: true,
            show: 'clip',
            //hide: 'slide',
            //show: { effect: "clip", duration: 500 },
            //hide: {
            //effect: "explode",
            //easing: "easeInExpo",
            //pieces: 4,
            //duration: 1000
            //},
            //hide: { effect: "explode", duration: 500 },
            //position: { my: "center", at: "top" },
            //position: "fixed",
            //position: {
            //    my: "center middle",
            //    at: "center middle",
            //    of: window
            //},
            autoResize: true,
            resizable: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            },
            create: function () {
                const self = $(this).closest('div.ui-dialog');
                const btnHeaderClose = self.find('.ui-dialog-titlebar-close');
                btnHeaderClose.attr({ id: btnCloseDailogId });
               // btnHeaderClose.addClass('btn btn-primary');
                self.find('.ui-dialog-titlebar-close')
                    .click(function (e) {
                        $(`#${dailogId}`).empty();
                        e.preventDefault();
                    });

            },
            buttons: buttonArr,
            open: function (event, ui) {
                $('.ui-widget-overlay').bind('click', function () {
                    $(`#${dailogId}`).empty();
                    $('#' + dailogId).dialog('close');
                });
                $(event.target).dialog('widget')
                    .css({ position: 'fixed' })
                    .position({ my: 'center', at: 'center', of: window });
            },
            close: function (e) {
                $(`#${dailogId}`).empty();
                $(`#${dailogId}`).dialog('option', 'height', 'auto');
                const evElem = document.getElementById(btnCloseDailogId);
                const event = new CustomEvent("DAILOG_CLOSE", { detail: { type: `type__CLOSE` } });
                evElem.dispatchEvent(event);
            },
        });
        return dialogElem;
    };

    const openModel = (response, fileName, isBlob = true, isPopup = true, appendId, blobName, storageContainerType, isIcon = true) => {
        try {
            const icon = getFileIcon(fileName);
            let tag = '<p>No Preview Found</p>';
            const height = window.innerHeight - 50;
            const imgHeight = height - 150;
            $('#file-model-content-file').css("height", `${height}px`);

            const dailogEventId = 'dailog-btn-download';
            const dialogElem = createDailog({ dailogId });
            /*if (!isBlob) {
                document.getElementById(dailogEventId).addEventListener('downloadFile', event => {
                    const { reqData, type } = event?.detail;
                    if (reqData && reqData?.url)
                        downloadFileUrl(reqData?.url, reqData?.blobName);
                        //downloadURI(reqData?.url, reqData?.blobName);
                }, false);
            }*/


            if (icon.trim() === FILES_TYPE.IMAGE) {
                if (isBlob && isPopup) {
                    blobToBase64(response).then(res => {
                        /*
                        tag = `<img src="${res}"  width="100%" height="${imgHeight}px"/>`;
                        $('#viewFilesBody').empty().append(tag);
                        $('#viewFiles').modal('show');
                        */
                        tag = `<img class="files-dailog-image" src="${res}" />`;
                        $(`#${dailogId}`).empty().append(tag);
                        dialogElem.dialog("option", "height", height);
                        dialogElem.dialog({
                            classes: {
                                "ui-dialog": "files-dailog-container"
                            }
                        });
                        dialogElem.dialog("open");
                    });
                }
                if (!isPopup && appendId && !isIcon) {
                    const $append = $(`#${appendId}`);
                    const parentElem = document.querySelector('[data-parent="data-parent"]')
                    parentElem.style.display = 'block';
                    tag = `<img src="${response.url}"/>`;
                    $append.empty().append(tag);
                }
                else if (!isPopup && appendId) {
                    const $append = $(`#${appendId}`);
                    const img = $('<img />').attr({ 'src': response, id: 'thumbnailImg' })
                        .addClass('img-fluid form-control img-size');
                    $append.empty().append(img);
                }
                else {
                    //tag = `<img src="${response?.url}" width="100%" height="${imgHeight}px"/>`;
                    //$('#viewFilesBody').empty().append(tag);
                    //$('#viewFiles').modal('show');
                    /* const tag = document.createElement('iframe');
                     tag.width = "100%";
                     tag.height = "100%";
                     //tag.classList.add('files-dailog-image');
                     tag.src = response?.url;*/
                    //iframe.id = hiddenIFrameID;
                    //tag.style.display = 'none';
                    tag = `<img class="files-dailog-image" src="${response?.url}" />`;
                    $(`#${dailogId}`).empty().append(tag);
                    //$(`#${dailogId}`).css('z-index', '9999999999');
                    dialogElem.dialog({
                        classes: {
                            "ui-dialog": "files-dailog-container"
                        }
                    });
                    dialogElem.dialog("option", "height", height);
                    dialogElem.dialog("open");
                }
            }
            else if (icon.trim() === FILES_TYPE.PDF) {
                let fileURL = isBlob ? URL.createObjectURL(response) : (response?.url ?? '');
                //document.title = fileName;
                //fileURL += `#filename=${fileName}`;
                //fileURL += `filename=${fileName}`;
                if (!isPopup && appendId && isIcon) {
                    const $append = $(`#${appendId}`);
                    const downThumId = 'downloadThumbnailId';
                    const attchment = $('<span />').addClass('full-column').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                    $append.append(attchment);
                    $("#" + downThumId).click(function (event) {
                        jqClient.ViewFile({ blobName: blobName, storageContainerType, fileName: fileName, isView: true });
                    });
                }
                else if (!isPopup && appendId && !isIcon) {
                    const $append = $(`#${appendId}`);
                    const parentElem = document.querySelector('[data-parent="data-parent"]')
                    parentElem.style.display = 'block';
                    tag = `<embed src="${fileURL}" id="fileurl" frameborder="0" width="100%" height="100%" title="${fileName}" />`;
                    $append.empty().append(tag);
                }
                else {
                    tag = `<embed src="${fileURL}" id="fileurl" frameborder="0" width="100%" height=100%" title="${fileName}" />`;
                    $(`#${dailogId}`).empty().append(tag);
                    dialogElem.dialog("option", "height", height);
                    dialogElem.dialog({
                        classes: {
                            "ui-dialog": "files-dailog-container"
                        }
                    });
                    dialogElem.dialog("open");
                }
            }
            else if (icon.trim() === FILES_TYPE.VIDEO) {
                const fileURL = isBlob ? URL.createObjectURL(response) : (response?.url ?? '');
                //document.title = fileName;
                //fileURL += `#filename=${fileName}`;
                //fileURL += `filename=${fileName}`;
                if (isBlob && isPopup) {
                    /*
                    tag = `<video width="100%" height="${imgHeight}px" controls autoplay>
                      <source src="${fileURL}" type="${response.type}">
                      Your browser does not support the video tag.
                    </video>`;
                    $('#viewFilesBody').empty().append(tag);
                    $('#viewFiles').modal('show');
                    */
                    tag = `<video class="files-dailog-video" controls autoplay>
                      <source src="${fileURL}" type="${response.contentType}">
                      Your browser does not support the video tag.
                    </video>`;
                    $(`#${dailogId}`).empty().append(tag);
                    dialogElem.dialog("option", "height", height);
                    dialogElem.dialog({
                        classes: {
                            "ui-dialog": "files-dailog-container"
                        }
                    });
                    dialogElem.dialog("open");
                }
                else if (!isPopup && appendId && isIcon) {
                    const $append = $(`#${appendId}`);
                    const downThumId = 'downloadThumbnailId';
                    const attchment = $('<span />').addClass('full-column').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                    $append.append(attchment);
                    $("#" + downThumId).click(function (event) {
                        jqClient.ViewFile({ blobName: blobName, storageContainerType, fileName: fileName, isView: true });
                    });
                }
                else if (!isPopup && appendId && !isIcon) {
                    const $append = $(`#${appendId}`);
                    const parentElem = document.querySelector('[data-parent="data-parent"]')
                    parentElem.style.display = 'block';
                    tag = `<video width="100%" height="${imgHeight}px" controls autoplay>
                      <source src="${fileURL}" type="${response.contentType}">
                      Your browser does not support the video tag.
                    </video>`;
                    $append.empty().append(tag);
                }
                else {
                    tag = `<video class="files-dailog-video" controls autoplay>
                      <source src="${fileURL}" type="${response.contentType}">
                      Your browser does not support the video tag.
                    </video>`;
                    $(`#${dailogId}`).empty().append(tag);
                    dialogElem.dialog("option", "height", height);
                    dialogElem.dialog({
                        classes: {
                            "ui-dialog": "files-dailog-container"
                        }
                    });
                    dialogElem.dialog("open");
                }
            }
            else if (icon.trim() === FILES_TYPE.MP3) {
                const fileURL = isBlob ? URL.createObjectURL(response) : (response?.url ?? '');
                if (isBlob && isPopup) {
                    tag = `<video width="100%" height="${imgHeight}px" controls autoplay>
                      <source src="${fileURL}" type="${response.type}">
                      Your browser does not support the video tag.
                    </video>`;
                    $('#viewFilesBody').empty().append(tag);
                    $('#viewFiles').modal('show');
                }
                else if (!isPopup && appendId && isIcon) {
                    const $append = $(`#${appendId}`);
                    const downThumId = 'downloadThumbnailId';
                    const attchment = $('<span />').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                    $append.append(attchment);
                    $("#" + downThumId).click(function (event) {
                        jqClient.ViewFile({ blobName: blobName, storageContainerType, fileName: fileName, isView: true });
                    });
                }
                else if (!isPopup && appendId && !isIcon) {
                    const $append = $(`#${appendId}`);
                    const parentElem = document.querySelector('[data-parent="data-parent"]')
                    parentElem.style.display = 'block';
                    tag = `<video width="100%" height="${imgHeight}px" controls autoplay>
                      <source src="${fileURL}" type="${response.contentType}">
                      Your browser does not support the video tag.
                    </video>`;
                    $append.empty().append(tag);
                }
                else {
                    tag = `<video width="100%" height="${imgHeight}px" controls autoplay>
                      <source src="${fileURL}" type="${response.contentType}">
                      Your browser does not support the video tag.
                    </video>`;
                    $(`#${dailogId}`).empty().append(tag);
                    dialogElem.dialog("option", "height", height);
                    dialogElem.dialog({
                        classes: {
                            "ui-dialog": "files-dailog-container"
                        }
                    });
                    dialogElem.dialog("open");
                }
            }
            else {
                if (!isPopup && appendId && !isIcon) {
                    commonUtil.downloadFileUrl(response.url, fileName);
                }
                else if (!isPopup && appendId) {
                    const $append = $(`#${appendId}`);
                    const downThumId = 'downloadThumbnailId';
                    const attchment = $('<span />').addClass('full-column').attr({ id: downThumId, 'attchment-key': blobName }).addClass(`downloadAttachment`).html(`${icon} <br/> ${fileName}`);
                    $append.append(attchment);
                    $("#" + downThumId).click(function (event) {
                        event.preventDefault();
                        jqClient.ViewFile({ blobName: blobName, storageContainerType, fileName: fileName, isView: true });
                    });
                }
            }
        } catch (e) { }
    }

    const closeModel = (id, contentId) => {
        document.getElementById(id).addEventListener('click', () => {
            $(`#${contentId}`).empty();
        });
    };

    const openContentModel = (html, title) => {
        $('#viewFilesBody').empty().append(html);
        $('#viewFiles').modal('show');
    };

    const getLastWord = (val, char) => {
        return /[^-]*$/.exec(val)[0];
    };

    const dateRangeValidation = (startDateId, endDateId) => {
        const ed = new Date();

        $('#' + startDateId).datepicker({
            format: 'mm/dd/yyyy',
            //endDate: ed,
            changeMonth: true,
            changeYear: true,
            daysOfWeekHighlighted: "5,6",
            autoclose: true,
            todayHighlight: true
        });

        $('#' + endDateId).datepicker({
            format: 'mm/dd/yyyy',
            //  endDate: ed,
            changeMonth: true,
            changeYear: true,
            daysOfWeekHighlighted: "5,6",
            autoclose: true,
            todayHighlight: true
        });

        $('#' + startDateId).on('changeDate', function (selected) {
            var startDate = new Date(selected.date.valueOf());
            $('#' + endDateId).datepicker('setStartDate', startDate);
        }).on('clearDate', function (selected) {
            $('#' + endDateId).datepicker('setStartDate', null);
        });

        $('#' + endDateId).on('changeDate', function (selected) {
            var endDate = new Date(selected.date.valueOf());
            $('#' + startDateId).datepicker('setEndDate', endDate);
        }).on('clearDate', function (selected) {
            $('#' + startDateId).datepicker('setEndDate', null);
        });
    }

    stringFormat = function (format) {
        const args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };



    const toArabicDigits = (val) => {
        const arabicDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
        return val.toString().replace(/[0-9]/g, function (w) {
            return arabicDigits[+w]
        });
    };

    const reverseString = (str) => {
        return str.split("").reverse().join("");
    };

    const animateValue = (obj, start, end, duration) => {
        try {
            duration = duration || 1000;
            let startTimestamp = null;
            const step = (timestamp) => {
                if (!startTimestamp) startTimestamp = timestamp;
                const progress = Math.min((timestamp - startTimestamp) / duration, 1);
                obj.innerHTML = Math.floor(progress * (end - start) + start);
                if (progress < 1) {
                    window.requestAnimationFrame(step);
                }
            };
            window.requestAnimationFrame(step);
        } catch (e) {

        }
    };

    const cssLoader = ({ id, appendId, isHide = false, clazz = '' }) => {
        _id = id || uuidv4();
        const loaderId = `css-item-loader-${_id}`;
        const loadeElem = document.querySelector(`[id="${loaderId}"]`);

        const template = `<div class='lds-ellipsis ${clazz}' id='${loaderId}'><div></div><div></div><div></div><div></div></div>`;
        if (id && isHide) {
            $('#' + loaderId).remove();
            return;
        }
        else if (loadeElem) return true;
        else if (appendId) {
            $('#' + appendId).append(template);
            return;
        }
        return { template, id: _id };
    };

    const getFutureDate = (date, days) => {
        moment.locale("en");
        return moment(date, commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD).add(days, 'days').format(commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
    };

    const getPastDate = (date, days) => {
        moment.locale("en");
        return moment(date, commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD).subtract(days, 'days').format(commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
    };

    const getMimeType = (fileName) => {
        if (!fileName) return '';

        const extension = fileName.split('.')[1];
        return MIME_TYPES[extension];
    };

    const downloadFileUrl = (downloadUrl, filename) => {
        if (filename) {
            const icon = getFileIcon(filename);
            // use HTML5 a[download] attribute to specify filename
            let a = document.createElement("a");
            // safari doesn't support this yet
            if (typeof a.download === 'undefined') {
                window.location.href = downloadUrl;
            } else {
                a.href = downloadUrl;
                if (icon.trim() === FILES_TYPE.IMAGE
                    || icon.trim() === FILES_TYPE.PDF) {
                    a.target = '_blank';
                }
                //a.title = filename;
                a.download = filename;
                document.body.appendChild(a);
                a.click();
            }
        } else {
            window.location.href = downloadUrl;
        }
        setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
        return false;
    };

    const downloadURI = (uri, filename) => {
        /* $('<form></form>')
             .attr('action', uri)
             .appendTo('body').submit().remove();*/
        var hiddenIFrameID = 'hiddenDownloader',
            iframe = document.getElementById(hiddenIFrameID);
        if (iframe === null) {
            iframe = document.createElement('iframe');
            iframe.id = hiddenIFrameID;
            iframe.style.display = 'none';
            document.body.appendChild(iframe);
        }
        iframe.src = uri;
    };

    const customFileUpload = (id, clazz) => {
        $('#' + id).on("change", function () {
            var fileLabel = $(this).next(`.${clazz}`);
            var files = $(this)[0].files;
            if (files.length > 1) {
                fileLabel.html(files.length + ' files selected');
            }
            else if (files.length == 1) {
                fileLabel.html(files[0].name);
            }
        });
    };

    const manageTextEditorUi = (arg, containerId) => {
        if (containerId) {
            $(`#${containerId} .jqte`).each(function (i, obj) {
                const id = `text-editor-${arg ?? i}`;
                $(obj).addClass('jqte-control').css({ 'padding': '0', 'margin': '0' }).attr({ id: id, name: id });
            });
        }
        else {
            $('.jqte').each(function (i, obj) {
                const id = `text-editor-${arg ?? i}`;
                $(obj).addClass('jqte-control').css({ 'padding': '0', 'margin': '0' }).attr({ id: id, name: id });
            });
        }
    };

    const btnProgress = (btnId, isStope) => {
        const btnElem = document.querySelector(`[id= "${btnId}"]`);
        if (!btnElem) return;

        btnElem.setAttribute("disabled", "disabled");
        //twoToneButton.innerHTML = btnLabel;
        btnElem.classList.add('spinning');

        if (isStope) {
            btnElem.classList.remove('spinning');
            btnElem.removeAttribute("disabled");
            //twoToneButton.innerHTML = btnLabel;
        }
    };

    const inlineElementProgress = (appendId, isStope, topPos, leftPos) => {
        const elem = document.querySelector(`[id= "${appendId}"]`);
        if (!elem) return;

        const divId = `inline-loader-${appendId}`;
        //elem.classList.add('loader');
        if (isStope) {
            //elem.classList.remove('loader');
            const loaderDivElem = document.getElementById(divId);
            if (loaderDivElem)
                elem.removeChild(loaderDivElem);

            return;
        }

        const loaderDiv = document.createElement('div');
        loaderDiv.id = divId;
        loaderDiv.classList.add('inline-loader');
        if (topPos) {
            loaderDiv.style.top = topPos;
        }
        if (leftPos) {
            loaderDiv.style.left = leftPos;
        }
        elem.appendChild(loaderDiv);
    };

    //Infinite Scroll
    const infiniteScroll = (customScrollHeight, callback) => {
        customScrollHeight = customScrollHeight ?? .6;
        $(window).scroll(function () {
            if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * customScrollHeight) {
                callback();
            }
        });

    };

    const infiniteScrollInsideElement = (elementId, customScrollHeight, callback) => {
        customScrollHeight = customScrollHeight ?? 50;
        const $elem = $(`#${elementId}`);
        $elem.scroll(function () {
            const $this = $(this);
            const height = this.scrollHeight - $this.height(); // Get the height of the div
            const scroll = $this.scrollTop(); // Get the vertical scroll position

            const isScrolledToEnd = (scroll >= (height - customScrollHeight));
            if (isScrolledToEnd) {
                callback();
            }
        });
    };

    const reverseInfiniteScroll = (elementId, customScrollHeight, callback) => {
        customScrollHeight = customScrollHeight ?? 100;
        const $elem = $(`#${elementId}`);
        $elem.scrollTop($elem.prop("scrollHeight"));
        $elem.scroll(function () {
            const $this = $(this);
            if ($this.scrollTop() < customScrollHeight) {
                callback();
            }
        });
    };

    const getTimeFilename = (fileName) => {
        const today = new Date();
        const y = today.getFullYear();
        const m = today.getMonth() + 1;
        const d = today.getDate();
        const h = today.getHours();
        const mi = today.getMinutes();
        const s = today.getSeconds();
        return y + "-" + m + "-" + d + "-" + h + "-" + mi + "-" + s + "." + fileName.split('.')[1];
    };

    const resetStars = (stars) => {
        for (let i = 0; i < stars.length; i++) {
            const elem = stars[i];
            if (elem) {
                elem.classList.remove("fa-star");
                elem.classList.add("fa-star-o");
            }
        }
    };

    const fillStars = (event, stars, callback) => {
        const { id } = event.target;

        const mapper = {
            one: 1,
            two: 2,
            three: 3,
            four: 4,
            five: 5
        };
        const id2num = mapper[id];

        for (let i = 0; i < id2num; i++) {
            const elem = stars[i];
            if (elem) {
                elem.classList.remove("fa-star-o");
                elem.classList.add("fa-star");
            }
        }
        if (callback) {
            const eventData = { rating: id2num };
            callback({ eventVal: event, eventData });
        }
    };

    const handleStarClickEvent = (event, clazz) => {
        clazz = clazz ?? "app-rate";
        const stars = document.getElementsByClassName(clazz);
        resetStars(stars);
        fillStars(event, stars);
    };

    const scrollToBottom = (elementId) => {
        const scrollElement = document.getElementById(elementId);
        scrollElement.scrollTop = scrollElement.scrollHeight;
    };



    const openFullPopup = () => {
        const popupElem = document.getElementById(viewDetailsContainerId);
        popupElem.style.height = "100%";
        popupElem.style.display = "flex";
    };

    const closeFullPopup = ({ btnId, eventName, data }) => {
        const popupElem = document.getElementById(viewDetailsContainerId);
        popupElem.style.height = "0%";
        popupElem.style.display = "none";
        /*  const evElem = document.getElementById(btnId);
          const event = new CustomEvent(eventName, { detail: { type: `type__${eventName}`, data: data } });
          evElem.dispatchEvent(event);*/

    };

    const getMimeTypes = (commaSepratedString) => {
        let list = [];
        commaSepratedString.split(',').forEach(d => {
            if (d.trim().toLowerCase()) {
                const mimeType = MIME_TYPES[d.trim().toLowerCase()];
                const isExsit = list.find(f => f == mimeType);
                if (!isExsit && mimeType)
                    list.push(mimeType);
            }
            
        });
        return list.join(',');
    };

    const copyTextToClipboard = (text, bgKey) => {
        var textArea = document.createElement("textarea");
        textArea.value = text;
        textArea.setAttribute("data-key", bgKey);
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand('copy');
        document.body.removeChild(textArea);
    };

    // When the user scrolls down 20px from the top of the document, show the button
    const scrollFunction = (btnMoveTopId) => {
        const btnMoveTopElm = document.getElementById(btnMoveTopId);
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            btnMoveTopElm.style.display = "block";
        } else {
            btnMoveTopElm.style.display = "none";
        }
    };

    const moveOnTop = () => {
        //document.body.scrollTop = 0;
        //document.documentElement.scrollTop = 0;
        $("html, body").animate({ scrollTop: 0 }, "slow");
    };

    const contains = (arr, v) =>{
        for (let i = 0; i < arr.length; i++) {
            if (arr[i] === v) return true;
        }
        return false;
    };

    const unique = (array) => {
        let arr = [];
        for (let i = 0; i < array.length; i++) {
            if (!contains(arr, array[i])) {
                arr.push(array[i]);
            }
        }
        return arr;
    };

    const arrayToJson = (arr, jsonKeyAttr, jsonValueAttr) => {
      return arr.reduce((json, item, key) => {
          json[item[jsonKeyAttr]] = item[jsonValueAttr];
          return json;
        }, {});
    };
    const GetDaysBetweenDates = (date1,date2) => {
        const datefrom = new Date(date1);
        const dateTo = new Date(date2);
        var diff = dateTo.getTime() - datefrom.getTime();
        var diffDays = diff / (1000 * 60 * 60 * 24);
        return diffDays;
    }
    

    let result = {};
    result.getMimeTypes = getMimeTypes;
    result.CONTENT_DAILOG_ID = contentDailogId;
    result.BTN_DAILOG_CLOSE_ID = btnCloseDailogId;
    result.APP_CONTENT_MODAL_BODY_ID = appContentModalBodyId;
    result.APP_CONTENT_MODAL_ID = appContentModalId;
    result.EVT_DAILOG_CLOSE = EVT_DAILOG_CLOSE;
    result.BTN_FULL_POPUP_CLOSE_ID = closeFullPopupId;
    result.CONTAINER_TYPE = CONTAINER_TYPE;
    result.STATUS_MASTER_TYPE = STATUS_MASTER_TYPE;
    result.STATUS_TYPE = STATUS_TYPE;
    result.FILES_TYPE = FILES_TYPE;
    result.DATE_FORMAT = DATE_FORMAT;
    result.MIME_TYPES = MIME_TYPES;
    result.getLocalUtcDateString = getLocalUtcDateString;
    result.getLocalUtcDateStringAr = getLocalUtcDateStringAr;
    result.getLocalUtcDateStringEn = getLocalUtcDateStringEn;
    result.formatter = formatter;
    result.getAllUrlParams = getAllUrlParams;
    result.getTextDirection = getTextDirection;
    result.downloadBase64File = downloadBase64File;
    result.isRTL = isRTL;
    result.getFileIcon = getFileIcon;
    result.toMMDDYYYY = toMMDDYYYY;
    result.toFormatedDate = toFormatedDate;
    result.concatDateNTime = concatDateNTime;
    result.isValidEmail = isValidEmail;
    result.toDateTime = toDateTime;
    result.isJsonString = IsJsonString;
    result.serverError = serverError;
    result.activateAdminTab = activateAdminTab;
    result.stringToBoolean = stringToBoolean;
    result.showThumbnail = showThumbnail;
    result.showThumbnailFromAppFolder = showThumbnailFromAppFolder;
    result.getFileIconClazz = getFileIconClazz;
    result.openModel = openModel;
    result.createDailog = createDailog;
    result.openContentModel = openContentModel;
    result.getLastWord = getLastWord;
    result.getImagesFormat = getImagesFormat;
    result.getLocalUtcDateEn = getLocalUtcDateEn;
    result.getLocalUtcDateAr = getLocalUtcDateAr;
    result.getMonthStartDateNEndDate = getMonthStartDateNEndDate;
    //result.getContainers = getContainers;
    result.dateRangeValidation = dateRangeValidation;
    result.stringFormat = stringFormat;
    result.createProgressBar = createProgressBar;
    result.createLoader = createLoader;
    result.toArabicDigits = toArabicDigits;
    result.createSectionLoader = createSectionLoader;
    result.reverseString = reverseString;
    result.animateValue = animateValue;
    result.cssLoader = cssLoader;
    result.uuidv4 = uuidv4;
    result.blobToBase64 = blobToBase64;
    result.base64toBlob = base64toBlob;
    result.getFutureDate = getFutureDate;
    result.getPastDate = getPastDate;
    result.getMimeType = getMimeType;
    result.downloadFileUrl = downloadFileUrl;
    result.closeModel = closeModel;
    result.customFileUpload = customFileUpload;
    result.manageTextEditorUi = manageTextEditorUi;
    result.btnProgress = btnProgress;
    result.inlineElementProgress = inlineElementProgress;
    result.infiniteScroll = infiniteScroll;
    result.infiniteScrollInsideElement = infiniteScrollInsideElement;
    result.reverseInfiniteScroll = reverseInfiniteScroll;
    result.getTimeFilename = getTimeFilename;
    result.resetStars = resetStars;
    result.fillStars = fillStars;
    result.handleStarClickEvent = handleStarClickEvent;
    result.scrollToBottom = scrollToBottom;
    result.closeFullPopup = closeFullPopup;
    result.openFullPopup = openFullPopup;
    result.copyTextToClipboard = copyTextToClipboard;
    result.scrollFunction = scrollFunction;
    result.moveOnTop = moveOnTop;
    result.unique = unique;
    result.arrayToJson = arrayToJson;
    result.addDayToDate = addDayToDate;
    result.toFormateMoment = toFormateMoment;
    result.getOnlyTime = getOnlyTime;
    result.getDataType = getDataType;
    result.PERMISSIONS_KEYS = PERMISSIONS_KEYS;
    result.GetDaysBetweenDates = GetDaysBetweenDates;


    return result;

})();