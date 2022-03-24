<template>
    <teleport to="body" v-if="visible">
        <div :class="[$style.loadingMask, 'd-flex align-items-center justify-content-center']">
            <div
                :class="[
                    $style.loadingContainer,
                    'd-flex flex-column align-items-center justify-content-center py-4 px-5 color-gray-100 font18',
                ]"
            >
                頁面載入中...
            </div>
        </div>
    </teleport>
</template>

<script>
import { render, h } from "vue";
const Loading = {
    name: "Loading",
    props: {
        // 是否顯示 Loading
        visible: {
            type: Boolean,
            default: false,
            required: true,
        },
    },
    setup() {
        return {};
    },
};

Loading.show = () => {
    const container = document.createElement("div");
    const vnode = h(Loading, {
        visible: true,
    });
    render(vnode, container);
    Loading.cachedContainer = container
}

Loading.hide = () => {
    render(null, Loading.cachedContainer);
    Loading.cachedContainer = null
}
export default Loading
</script>

<style module src="./Loading.scss" />
