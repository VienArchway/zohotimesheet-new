<script setup>
import {onMounted, ref} from 'vue'
import {useI18n} from 'vue-i18n'
import { getVerifyTokenApi } from '@/api/resources/ZohoToken.js'

const { t } = useI18n()
const status = ref(null)

onMounted(async () => {
  status.value = await getVerifyTokenApi()
})

</script>

<template>
  <div>
    <b>authorized: {{ status }}</b>
    <h1>Zoho index page</h1>
    {{ t('hello') }}
    <div>
      <router-link data-cy="link-about" to="/about">About</router-link>
    </div>
    
    <v-container class="grey lighten-5">
        <v-row>
            <v-col cols="12" md="4">
                <v-card class="pa-2" outlined tile>
                    <v-card-title>
                    {{ t('project') }}
                    <v-spacer></v-spacer>
                    <v-text-field
                        v-model="values.projectData.projectNameFilter"
                        append-icon="mdi-magnify"
                        :label="t('search')"
                        @change="projectNameFilter"
                        single-line
                        hide-details
                    ></v-text-field>
                    </v-card-title>
                        <v-table 
                            fixed-header
                            height="300px"
                        >
                            <thead>
                                <tr>
                                    <th class="text-left">
                                    </th>
                                    <th class="text-left">
                                        {{ t('project') }}
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="item in values.projectData.filterItems"
                                    :key="item.projId"
                                >
                                    <td>
                                        <v-checkbox
                                            v-model="searchConditions.projects"
                                            :value="item"
                                            hide-details
                                        ></v-checkbox>
                                    </td>
                                    <td>{{ item.projName }}</td>
                                </tr>
                            </tbody>
                    </v-table>
                </v-card>
            </v-col>
            <v-col cols="12" md="8">
                <v-row>
                    <v-col cols="12" md="12">
                        <v-select
                                v-model="searchConditions.sprintTypeId"
                                :items="values.sprintData.items"
                                :label="t('sprint')"
                                item-text="text"
                                item-value="value"
                                dense
                                outlined
                            ></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" md="6">
                        <v-text-field
                            v-model="searchConditions.startDate"
                            :label="t('startdate')"
                            prepend-icon="far fa-calendar-alt"
                            v-on="on"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field
                            v-model="searchConditions.endDate"
                            :label="t('enddate')"
                            prepend-icon="far fa-calendar-alt"
                            v-on="on"
                        ></v-text-field>
                        <!-- <v-menu
                            v-model="isShowEndDate"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            transition="scale-transition"
                            offset-y
                            min-width="290px"
                        >
                            <template v-slot:activator="{ on }">
                                <v-text-field
                                    :label="('enddate')"
                                    prepend-icon="far fa-calendar-alt"
                                    readonly
                                    v-on="on"
                                    :value="customEndDateFormatter"
                                ></v-text-field>
                            </template>
                            <v-date-picker
                                v-model="searchConditions.endDate"
                                no-title
                                @input="isShowEndDate = false"
                                :min="searchConditions.minDate"
                                :max="searchConditions.maxDate"
                            ></v-date-picker>
                        </v-menu> -->
                    </v-col>
                </v-row>
            <v-row>
                <v-col>
                    <div>
                        <v-btn class="ma-2" color="success" @click="upload" :disabled="values.projectData.items.length == 0 || !searchConditions.projects.length">{{ ('upload') }}</v-btn>
                    </div>
                </v-col>
            </v-row>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12" md="12">
                <v-table v-show="values.logworkData.length > 0 || values.logworkData !== undefined || values.logworkData !== null"
                    fixed-header
                    height="400px" >
                    <thead>
                        <tr>
                            <th class="text-left">
                                {{ t("project") }}
                            </th>
                            <th class="text-left">
                                {{ t("item") }}
                            </th>
                            <th class="text-left">
                                {{ t("owner") }}
                            </th>
                            <th class="text-left">
                                {{ t("logdate") }}
                            </th>
                            <th class="text-left">
                                {{ t("logtime") }}
                            </th>
                            <th class="text-left">
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr
                            v-for="item in values.logworkData.items"
                            :key="item.logTimeId"
                        >
                            <td>{{ item.projName }}</td>
                            <td>{{ item.itemName }}</td>
                            <td>{{ item.OwnerName }}</td>
                            <td>{{ item.logDate }}</td>
                            <td>{{ item.logTime }}</td>
                            <td>
                                <v-tooltip bottom>
                                    <template v-slot:activator="{ props }">
                                        <v-btn
                                        color="primary"
                                        dark
                                        v-bind="props"
                                        >
                                        {{ t('json')}}
                                        </v-btn>
                                    </template>
                                    <span>{{ item }}</span>
                                    <!-- <vue-json-pretty
                                        :data="item"
                                    /> -->
                                </v-tooltip>
                            </td>
                        </tr>
                    </tbody>
                </v-table>
            </v-col>
        </v-row>
    </v-container>
  </div>
</template>
<script>
import "./index.scss"
// import '@/vue-json-pretty/lib/styles.css'
import moment from "moment"
// import VueJsonPretty from 'vue-json-pretty'
import adlsApi from '@/api/resources/adls'
import projectApi from '@/api/resources/project'

export default {
    components: {
        // VueJsonPretty
    },
    data() {
        return {
            isShowStartDate: false,
            isShowEndDate: false,
            // currentLocale: this.$i18n.locale,
            urls: {
                projectApi: "api/project",
                adlsApi: "api/adls"
            },
            values: {
                projectData: {
                    projectNameFilter: "",
                    headers: [
                        {
                            text: "project",
                            value: "projName"
                        }
                    ],
                    items: [],
                    filterItems: []
                },
                sprintData: {
                    items: [
                        {text: "activesprint", value: 2 },
                        {text: "allsprint", value: 0 }
                    ]
                },
                logworkData: {
                    headers: [
                        { text: "project", value: "projName" },
                        { text: "item", value: "itemName" },
                        { text: "owner", value: "OwnerName" },
                        { text: "logdate", value: "logDate" },
                        { text: "logtime", value: "logTime" },
                        { text: "", value: "json", sortable: false }
                    ],
                    items: []
                }
            },
            searchConditions: {
                startDate: new Date().toISOString().substr(0, 10),
                endDate: new Date().toISOString().substr(0, 10),
                sprintTypeId: 2,
                projects: [],
                minDate: new Date(2019, 7, 2).toISOString().substr(0, 10),
                maxDate: new Date().toISOString().substr(0, 10)
            }
        };
    },
    async created() {
        // moment.locale(this.$i18n.locale);
        // this.$store.commit("showLoading");
        try {
            const resProject = await projectApi.getAll();
            this.values.projectData.items = resProject;
            this.values.projectData.filterItems = resProject;
        }catch (error) {
            console.log(error)
        }
        // finally {
        //     this.$store.commit("closeLoading");
        // }
    },
    computed: {
        customStartDateFormatter () {
            return this.searchConditions.startDate ? moment(this.searchConditions.startDate).format("LL") : "";
        },
        customEndDateFormatter () {
            return this.searchConditions.endDate ? moment(this.searchConditions.endDate).format("LL") : "";
        }
    },
    methods: {
        async upload() {
            // this.$store.commit("showLoading");
            try {

                this.values.logworkData.items = [];

                const searchCondition = {
                    startDate: new Date(this.searchConditions.startDate),
                    endDate: new Date(this.searchConditions.endDate),
                    sprintTypeId: this.searchConditions.sprintTypeId,
                    projects: this.searchConditions.projects
                };

                // const response = await axios.post(`${this.urls.adlsApi}/transfer`,searchCondition);
                const response = await adlsApi.transfer(searchCondition);
                if (response !== null && response !== undefined)
                {
                    this.values.logworkData.items = response;

                    // this.$store.commit("notify.success", { content: t("datauploaded"), timeout:10000 });
                }
            } finally {
                // this.$store.commit("closeLoading");
            }
        },
        projectNameFilter() {
            this.values.projectData.filterItems = _.filter(this.values.projectData.items, (proj) => { return proj.projName.includes(this.values.projectData.projectNameFilter)});
        }
    }


};
</script>

<route lang="yaml">
meta:
  layout: default
</route>
