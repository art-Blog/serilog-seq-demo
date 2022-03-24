<template>
    <h1>Detail</h1>
    <div>
        <ol>
            <li>Account {{ current.account }}</li>
            <li>Name {{ current.name }}</li>
            <li>Authority {{ current.authority }}</li>
        </ol>
    </div>
</template>

<script>

export default {
    name: 'DetailContainer',
    props: {
        model: {
            type: Object,
            default: {}
        }
    },
    computed: {
        current() {
            return this.model.admin
        }
    }
}

</script>

<style module src="./Detail.scss"></style>