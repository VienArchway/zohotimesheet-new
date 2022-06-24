<template>
  <v-container fluid>
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
                    @update:modelValue="projectNameFilter"
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
                        item-title="text"
                        item-value="value"
                        dense
                        outlined
                    />
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="12" md="6">
                    <v-text-field
                        v-model="searchConditions.startDate"
                        :label="t('startdate')"
                        prepend-icon="far fa-calendar-alt"
                    ></v-text-field>
                </v-col>
                <v-col cols="12" md="6">
                    <v-text-field
                        v-model="searchConditions.endDate"
                        :label="t('enddate')"
                        prepend-icon="far fa-calendar-alt"
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
                        <td>{{ item.ownerName }}</td>
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
</template>
<script>
import "./index.scss"
import moment from "moment"
import adlsApi from '@/api/resources/adls'
import projectApi from '@/api/resources/project'
import appStore from '@/store/app.js'
import { defineComponent } from 'vue'
import { useI18n } from 'vue-i18n'

const app = appStore()

export default defineComponent({
  setup() {
    const { t } = useI18n()
    return { t }
  },
  data() {
    return {
      isShowStartDate: false,
      isShowEndDate: false,
      values: {
        projectData: {
          projectNameFilter: "",
          headers: [
            {
              text: this.t("project"),
              value: "projName"
            }
          ],
          items: [],
          filterItems: []
        },
        sprintData: {
          items: [
            {text: this.t("activesprint"), value: 2 },
            {text: this.t("allsprint"), value: 0 }
          ]
        },
        logworkData: {
          headers: [
            { text: this.t("project"), value: "projName" },
            { text: this.t("item"), value: "itemName" },
            { text: this.t("owner"), value: "OwnerName" },
            { text: this.t("logdate"), value: "logDate" },
            { text: this.t("logtime"), value: "logTime" },
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
    try {
      await app.load(async () => {
        const resProject = await projectApi.getAll();

        this.values.projectData.items = resProject;
        this.values.projectData.filterItems = resProject;

      })
    } catch (error) {
      const resMessage = error.response?.data?.message;
      const errorDetail = JSON.parse(resMessage);
      if (errorDetail) {
        app.error(errorDetail.message, 100000);
      }
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
      try {
        await app.load(async () => {

          this.values.logworkData.items = [];

          const searchCondition = {
            startDate: new Date(this.searchConditions.startDate),
            endDate: new Date(this.searchConditions.endDate),
            sprintTypeId: this.searchConditions.sprintTypeId,
            projects: this.searchConditions.projects
          };

          const response = await adlsApi.transfer(searchCondition);

          if (response !== null && response !== undefined)
          {
            this.values.logworkData.items = response;
          }
        })
      } catch (error) {
        const resMessage = error.response?.data?.message;
        const errorDetail = JSON.parse(resMessage);
        if (errorDetail) {
          app.error(errorDetail.message, 100000);
        }
      } finally {
        app.success(this.t("dataloaded"), 5000);
      }
    },
    projectNameFilter() {
      this.values.projectData.filterItems = _.filter(this.values.projectData.items, (proj) => { return proj.projName.includes(this.values.projectData.projectNameFilter)});
    }
  }
})

</script>

<route lang="yaml">
meta:
  layout: default
</route>
