<template>
    <teleport to="body" v-if="visible">
        <div :class="[$style.modalMask, 'd-flex align-items-center justify-content-center']">
            <div :class="[$style.modalContainer, modalSize, wrapperClass]">
                <div v-if="title" :class="[$style.title, 'font18-bold p-3']">{{ title }}</div>
                <div v-if="isShowClose" :class="$style.closeBtn" @click="handleCancel" />
                <slot name="header" />
                <div :class="[contentClass, 'pt-4 pb-4 ps-3 pe-3']">
                    <slot />
                </div>
                <div
                    v-if="isShowFooter"
                    class="d-flex p-2 align-items-center justify-content-center"
                    :class="$style.footer"
                >
                    <button
                        type="button"
                        v-if="cancelText"
                        class="btn btn-gray flex-grow-1 me-1"
                        :class="$style.btn"
                        @click="handleCancel"
                    >
                        {{ cancelText }}
                    </button>
                    <button
                        type="button"
                        class="btn btn-primary flex-grow-1 ms-1"
                        :class="$style.btn"
                        @click="handleConfirm"
                    >
                        {{ confirmText }}
                    </button>
                </div>
            </div>
        </div>
    </teleport>
</template>

<script>
import { h, render } from "vue";
const Modal = {
    name: "Modal",
    props: {
        // 是否顯示 Modal
        visible: {
            type: Boolean,
            default: false,
            required: true,
        },
        // 右上角是否顯示關閉
        isShowClose: {
            type: Boolean,
            default: false,
        },
        // Modal Title
        title: {
            type: String,
            default: "",
        },
        // 是否顯示 footer
        isShowFooter: {
            type: Boolean,
            default: true,
        },
        // 確認按鈕文字
        confirmText: {
            type: String,
            default: "",
        },
        // 取消按鈕文字
        cancelText: {
            type: String,
            default: "",
        },
        // Modal wrapper class
        wrapperClass: {
            type: String,
            default: "",
        },
        // Modal content class
        contentClass: {
            type: String,
            default: "text-center",
        },
        // Modal 指定寬度，sizeSmall / sizeMid / sizeLarge
        modalSize: {
            type: String,
            default: "minWidth-Small",
        },
    },
    emits: ["close", "confirm", "cancel"],
    setup(props, { emit }) {
        const handleConfirm = () => {
            emit("confirm");
            handleCloseModal();
        };

        const handleCancel = () => {
            emit("cancel");
            handleCloseModal();
        };

        const handleCloseModal = () => {
            emit("close");
        };

        return {
            handleConfirm,
            handleCancel,
        };
    },
};

/**
 * @description 預設 modal 設定
 *  @param { string } title - modal 標題
 *  @param { string | hyperscript } content - modal 內容
 *  @param { string } contentClass - 設定全域 class, 預設文字置中
 *  @param { string } confirmText - 按鈕文字, 預設: '確認')
 *  @param { string } cancelText - 按鈕文字, 預設: '取消')
 *  @param { function } confirm - '確認' 按鈕 callback
 *  @param { function } cancel - '取消' 按鈕 callback
 */
const info = ({
    title = "",
    content = null,
    contentClass = "text-center",
    confirmText = "確認",
    cancelText = "取消",
    confirm = null,
    cancel = null,
    modalSize = "minWidth-Small",
}) => {
    const container = document.createElement("div");
    const vnode = h(
        Modal,
        {
            visible: true,
            title,
            confirmText,
            cancelText,
            contentClass,
            modalSize,
            onConfirm: () => {
                destroy();
                if (confirm && confirm instanceof Function) {
                    confirm();
                }
            },
            onCancel: () => {
                destroy();
                if (cancel && cancel instanceof Function) {
                    cancel();
                }
            },
        },
        content ? () => content : null
    );
    render(vnode, container);
    const destroy = () => {
        render(null, container);
    };
};

Modal.info = info;

Modal.alert = props => {
    info({
        cancelText: "",
        ...props,
    });
};

export default Modal;
</script>

<style module src="./Modal.scss" />
