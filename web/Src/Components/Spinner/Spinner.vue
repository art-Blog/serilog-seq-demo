<template>
    <div class="spinner" v-show="loading">
        <div class="d-flex justify-content-center">
            <div :class="[$style.spinnerStyle, $style.spinnerDelay1]"></div>
            <div :class="[$style.spinnerStyle, $style.spinnerDelay2]"></div>
            <div :class="[$style.spinnerStyle, $style.spinnerDelay3]"></div>
        </div>
    </div>
</template>
<script>
const Spinner = {
    name: 'Spinner',
    props: {
        loading: {
            type: Boolean,
            default: false,
        },
    }
}

export default Spinner
</script>
<style module src="./Spinner.scss"></style>
