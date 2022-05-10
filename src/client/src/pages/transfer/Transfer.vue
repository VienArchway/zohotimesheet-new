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
                    <v-data-table
                        v-model="searchConditions.projects"
                        :headers="values.projectData.headers"
                        :items="values.projectData.items"
                        :search="values.projectData.projectNameFilter"
                        :show-select="true"            
                        :disable-pagination="true"
                        hide-default-footer
                        item-key="id"
                        height="300px"
                        fixed-header
                    ></v-data-table>
                    <v-table>
                        <thead>
                        <tr>
                        <th class="text-left">
                            Name
                        </th>
                        <th class="text-left">
                            Calories
                        </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr
                            v-for="item in desserts"
                            :key="item.name"
                        >
                        <td>{{ item.name }}</td>
                        <td>{{ item.calories }}</td>
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
                <!-- <v-row>
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
                </v-row> -->
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
                    :headers="values.logworkData.headers"
                    :items="values.logworkData.items"
                    fixed-header
                    height="400px" >
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
import "./index.scss";
import moment from "moment";
import projectApi from '../../api/project.js'

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
                        {text: ("activesprint"), value: 2 },
                        {text: ("allsprint"), value: 0 }
                    ]
                },
                logworkData: {
                    headers: [
                        { text: ("project"), value: "projName" },
                        { text: ("item"), value: "itemName" },
                        { text: ("owner"), value: "OwnerName" },
                        { text: ("logdate"), value: "logDate" },
                        { text: ("logtime"), value: "logTime" },
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
            console.log(this.values.projectData.headers[0])
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
            this.$store.commit("showLoading");
            try {

                this.values.logworkData.items = [];

                const searchCondition = {
                    startDate: new Date(this.searchConditions.startDate),
                    endDate: new Date(this.searchConditions.endDate),
                    sprintTypeId: this.searchConditions.sprintTypeId,
                    projects: _.find(this.searchConditions.projects, { id: "" }) ? null : this.searchConditions.projects
                };

                const response = await axios.post(`${this.urls.adlsApi}/transfer`,searchCondition);
                if (response.data !== null && response.data !== undefined)
                {
                    this.values.logworkData.items = response.data;

                    this.$store.commit("notify.success", { content: ("datauploaded"), timeout:10000 });
                }
            } finally {
                this.$store.commit("closeLoading");
            }
        }
    }


};
</script>
