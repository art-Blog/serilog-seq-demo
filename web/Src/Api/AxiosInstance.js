import axios from "axios";
import { h, render } from "vue";
import Modal from "~Components/Modal/Modal";
import Loading from "~Components/Loading/Loading";
import Spinner from "~Components/Spinner/Spinner";


const showLoading = visible => {
    if (visible) {
        const vnode = h(Loading, {
            visible,
        });
        render(vnode, document.body);
    } else {
        render(null, document.body);
    }
};

const showSpinner = loading => {
    if (loading) {
        const vnode = h(Spinner, {
            loading,
        });
        render(vnode, document.querySelector("#loading-snipper"));
    } else {
        render(null, document.querySelector("#loading-snipper"));
    }
};

/**
 * @method axios POST
 * @description
 */
export const post = (
    url,
    postData,
    config = {
        useLoading: true,
        loadingStyle: "showLoading",
    }
) => {
    const { useLoading, loadingStyle } = config;
    try {
        if (useLoading) {
            loadingStyle === "showSpinner" ? showSpinner(true) : showLoading(true);
        }
        let data = null;
        if (postData) {
            data = new FormData();
            const setFormData = (value, nameArr = [], isArr = false) => {
                Object.keys(value).forEach(key => {
                    let dataNameArr = nameArr;

                    if (isArr) {
                        const lastKeyIndex = dataNameArr.length - 1;
                        const reg = /\[.+\]/;
                        dataNameArr[lastKeyIndex] = reg.test(dataNameArr[lastKeyIndex])
                            ? dataNameArr[lastKeyIndex].replace(reg, `[${key}]`)
                            : `${dataNameArr[lastKeyIndex]}[${key}]`;
                    } else dataNameArr = dataNameArr.concat(key);

                    if (value[key] instanceof Array) {
                        setFormData(value[key], dataNameArr, true);
                    } else if (value[key] instanceof Object) {
                        setFormData(value[key], dataNameArr);
                    } else if (value[key] !== null) {
                        const name = dataNameArr.join(".");
                        data.append(name, value[key]);
                    }
                });
            };
            setFormData(postData);
        }
        return axios({
            method: "post",
            url,
            data,
            processData: false,
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "content-type": "multipart/form-data",
            },
        })
            .then(response => {
                return response.data;
            })
            .catch(err => {
                catchError(err);
            })
            .finally(() => {
                if (useLoading) {
                    loadingStyle === "showSpinner" ? showSpinner(false) : showLoading(false);
                }
            });
    } catch (error) {
        onError(error);
    }
};

const catchError = err => {
    const { status, data } = err.response;
    switch (status) {
        case 401: {
            Modal.alert({
                content: data.msg,
            });
            break;
        }
        case 403: {
            Modal.alert({
                content: data.msg,
            });
            break;
        }
        case 500: {
            Modal.alert({
                content: data.msg,
            });
            break;
        }
        default: {
            return null;
        }
    }
};

const onError = error => {
    throw error.response;
};

/**
 * @method axios instance
 * @description
 */
const instance = axios.create({
    headers: {
        "X-Requested-With": "XMLHttpRequest",
    },
});

export default instance;
