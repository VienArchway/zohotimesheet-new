<template>
    <v-container class="grey lighten-5">
        <v-row>
            <v-col cols="12" md="4">
                <v-card class="pa-2" outlined tile>
                    <v-card-title>
                    {{ ('project') }}
                    <v-spacer></v-spacer>
                    <v-text-field
                        v-model="values.projectData.projectNameFilter"
                        append-icon="mdi-magnify"
                        :label="('search')"
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
                                        <v-checkbox
                                            v-model="searchConditions.projects"
                                            value="all"
                                            hide-details
                                        ></v-checkbox>
                                    </th>
                                    <th class="text-left">
                                        ProjName
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="item in values.projectData.items"
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
                                :label="('sprint')"
                                item-text="text"
                                item-value="value"
                                dense
                                outlined
                            ></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" md="6">
                        <v-menu
                            v-model="isShowStartDate"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            transition="scale-transition"
                            offset-y
                            min-width="290px"
                        >
                            <template v-slot:activator="{ on }">
                                <v-text-field
                                    :label="('startdate')"
                                    prepend-icon="far fa-calendar-alt"
                                    readonly
                                    v-on="on"
                                    :value="customStartDateFormatter"
                                ></v-text-field>
                            </template>
                            <v-date-picker
                                v-model="searchConditions.startDate"
                                no-title
                                @input="isShowStartDate = false"
                                :min="searchConditions.minDate"
                                :max="searchConditions.maxDate"
                            ></v-date-picker>
                        </v-menu>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
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
                        </v-menu>
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
                                project
                            </th>
                            <th class="text-left">
                                item
                            </th>
                            <th class="text-left">
                                owner
                            </th>
                            <th class="text-left">
                                logdate
                            </th>
                            <th class="text-left">
                                logtime
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
                                <v-menu
                                    offset-y
                                    bottom
                                    origin="center center"
                                    transition="scale-transition"
                                    open-on-hover
                                    >
                                    <template v-slot:activator="{ on }">
                                        <v-btn
                                            color="primary"
                                            dark
                                            v-on="on"
                                        >
                                            {{ ('json')}}
                                        </v-btn>
                                    </template>

                                    <v-list>
                                        <v-list-item>
                                            <vue-json-pretty
                                                :data="item"
                                            />
                                        </v-list-item>
                                    </v-list>
                                </v-menu>
                            </td>
                        </tr>
                    </tbody>
                    <template v-slot:[`item.json`]="{ item }">
                        <v-menu
                            offset-y
                            bottom
                            origin="center center"
                            transition="scale-transition"
                            open-on-hover
                            >
                            <template v-slot:activator="{ on }">
                                <v-btn
                                    color="primary"
                                    dark
                                    v-on="on"
                                >
                                    {{ ('json')}}
                                </v-btn>
                            </template>

                            <v-list>
                                <v-list-item>
                                    <vue-json-pretty
                                        :data="item"
                                    />
                                </v-list-item>
                            </v-list>
                        </v-menu>
                    </template>
                </v-table>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
import "./index.scss"
import moment from "moment";
import projectApi from '../../api/project.js'
import logworkApi from '../../api/logwork.js'

export default {
    components: {
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
                    items: []
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
            const resProject = await projectApi.getAll({ headers: { AccessToken: localStorage.accessToken } });
            this.values.projectData.items = resProject;
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
                const response = await logworkApi.find(searchCondition);
                if (response !== null && response !== undefined)
                {
                    this.values.logworkData.items = response;

                    this.$store.commit("notify.success", { content: ("datauploaded"), timeout:10000 });
                }
            } finally {
                // this.$store.commit("closeLoading");
            }
        }
    }


};
</script>
