<template>
  <div class="q-pa-md">
    <div class="row q-col-gutter-lg">
      <!-- projects list-->
      <div class="col-12 col-md-4 order-first order-md-first">
        <label>{{ t('sprint') }}</label>
        <q-select
            filled
            v-model="searchConditions.sprintTypeId"
            :options="values.sprintData.items"
            :label="t('sprint')"
            behavior="menu"
        />
        <div class="row q-col-gutter-md py-4">
          <div class="col-12 col-md-6">
            <label>{{ t('startdate') }}</label>
            <q-input
                filled
                dense
                v-model="searchConditions.startDate"
                mask="date"
                :rules="[val => !!val || '* Required', 'date']"
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy cover>
                    <q-date v-model="searchConditions.startDate" v-close-popup />
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
          <div class="col-12 col-md-6">
            <label>{{ t('enddate') }}</label>
            <q-input
                filled
                dense
                v-model="searchConditions.endDate"
                mask="date"
                :rules="[val => !!val || '* Required', 'date']"
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy cover ref="qEndDateProxy">
                    <q-date v-model="searchConditions.endDate" @click="() => $refs.qEndDateProxy.hide()" />
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
        </div>
        <q-table
            bordered
            dense
            class="transfer-project-tbl"
            :rows="values.projectData.filterItems"
            :columns="values.projectData.columns"
            :rows-per-page-options="[0]"
            :filter="filterProjectName"
            :loading="loadingTable"
            row-key="projName"
            selection="multiple"
            v-model:selected="selectedProjects"
        >
          <template v-slot:loading>
            <q-inner-loading showing color="teal" />
          </template>
          <template v-slot:top>
            <div>
              <q-input borderless filled dense color="primary" v-model="filterProjectName" placeholder="Search" clearable>
                <template v-slot:append>
                  <q-icon name="search" />
                </template>
              </q-input>
            </div>
            <q-space />
            <div>
              <q-btn flat icon="upload" style="color: #009688" :label="t('upload')" @click="upload" :disable="selectedProjects.length === 0" />
            </div>
          </template>
        </q-table>
      </div>
      
      <!-- sprint condition -->
      <div class="col-12 col-md-8 order-last order-md-last">
        <q-table
            class="q-table transfer-project-detail-tbl"
            :rows="values.logworkData.items"
            :columns="values.logworkData.headers"
            :filter="filterDetailProjectName"
            row-key="projName"
        >
          <template v-slot:top>
            <div style="width: 100%">
              <h6>Projects</h6>
              <q-input borderless filled dense debounce="300" color="primary" v-model="filterDetailProjectName" placeholder="Search">
                <template v-slot:append>
                  <q-icon name="search" />
                </template>
              </q-input>
            </div>
            
          </template>
        </q-table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onBeforeMount  } from 'vue'
import adlsApi from '@/api/resources/adls'
import projectApi from '@/api/resources/project'
import appStore from '@/store/app'
import { useI18n } from 'vue-i18n'

const app = appStore()
const { t } = useI18n()

const loadingTable = ref(false)
const filterProjectName = ref('')
const filterDetailProjectName = ref('')
const selectedProjects = ref([])
const values = reactive({
  projectData: {
    projectNameFilter: "",
    columns: [
      {
        name: 'Project',
        label: 'Project Name',
        align: 'left',
        sortable: true,
        field: row => row.projName
      }
    ],
    items: [],
    filterItems: [],
    selected: null
  },
  sprintData: {
    items: [
      { label: t("activesprint"), value: 2 },
      { label: t("allsprint"), value: 0 }
    ]
  },
  logworkData: {
    headers: [
      { label: t("project"), value: "projName", field: row => row.projName },
      { label: t("item"), value: "itemName", field: row => row.itemName },
      { label: t("owner"), value: "OwnerName", field: row => row.OwnerName },
      { label: t("logdate"), value: "logDate", field: row => row.logDate },
      { label: t("logtime"), value: "logTime", field: row => row.logTime },
      { label: "", value: "json", sortable: false }
    ],
    items: []
  }
})
const searchConditions = reactive({
  startDate: new Date().toISOString().substr(0, 10),
  endDate: new Date().toISOString().substr(0, 10),
  sprintTypeId: { label: t("activesprint"), value: 2 },
  minDate: new Date(2019, 7, 2).toISOString().substr(0, 10),
  maxDate: new Date().toISOString().substr(0, 10)
})

// handle a methods
async function upload() {
  await app.load(async () => {

    values.logworkData.items = [];

    const searchCondition = {
      startDate: new Date(searchConditions.startDate),
      endDate: new Date(searchConditions.endDate),
      sprintTypes: searchConditions.sprintTypeId.value === "2" ? ["2"] :["2", "3"],
      projects: selectedProjects.value
    };

    const response = await adlsApi.transfer(searchCondition);
    if (response !== null && response !== undefined)
    {
      values.logworkData.items = response;
    }
    app.success(t("dataloaded"), 5000);
  })
}

// bind data
onBeforeMount(async () => {
  loadingTable.value = true
  const resProject = await projectApi.getAll();
  
  values.projectData.items = resProject;
  values.projectData.filterItems = resProject;
  loadingTable.value = false
})

</script>

<style lang="sass">
.transfer-project-tbl
  height: calc(100vh - 320px)
  
.q-table
  tbody tr td
    height: 45px !important

.transfer-project-detail-tbl
  @media screen and (min-width: $screen-small)
    height: calc(100vh - 400px)
  @media screen and (min-width: $screen-medium)
    height: calc(100vh - 130px)
</style>