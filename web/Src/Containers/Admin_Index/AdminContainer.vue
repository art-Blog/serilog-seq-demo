<template>
    <input v-model="state.form.authority"/>
    <button @click="search">GO</button>

    <h1>列表</h1>
    <ul>
        <li v-for="admin in state.admins">
            <a :href="detailLink(admin.id)">{{ admin.account }} - {{ admin.name }}</a>
        </li>
    </ul>

</template>

<script setup>
import {ref, computed, onMounted, useCssModule, onBeforeUnmount, reactive} from "vue";
import {getAdminsByAuthority} from "~Api/AdminApi";

const state = reactive({
    admins: [],
    form: {
        authority: 10
    }
})

function detailLink(id) {
    return `/Admin/Detail/${id}`
}

async function search() {
    let {data, success} = await getAdminsByAuthority(state.form)
    if (success) {
        state.admins = data
    }
}

</script>

<style module src="./Admin.scss"></style>