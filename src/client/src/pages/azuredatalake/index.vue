<template>
  <div class="adls-table">
    <v-row>
        <v-col cols="9">
            <v-text-field
                v-model="data.logItemFilter"
                append-icon="mdi-magnify"
                :label="t('searchcondition')"
                @update:modelValue="search"
                hide-details
            ></v-text-field>
        </v-col>
        <v-col>
            <v-btn
                :disabled="!data.selectedItems.length"
                @click="remove"
                color="error"
                class="ma-2"
                right
            >
                {{ t('delete') }}
            </v-btn>
            <v-btn
                @click="dialog = true"
                color="warning"
                class="ma-2"
                right
            >
                {{ t('restore') }}
            </v-btn>
        </v-col>
    </v-row>
    <v-table
        fixed-header
        height="calc(100vh - 170px)"
    >
        <thead style="position: sticky; z-index: 2">
            <tr>
                <th class="text-left">
                </th>
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
                v-for="item in data.displayItems"
                :key="item.logTimeId"
            >
                <v-checkbox
                    v-model="data.selectedItems"
                    :value="item"
                    hide-details
                ></v-checkbox>
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
    <v-pagination
      v-model="page.current"
      :length="page.length"
      :total-visible="7"
      @update:model-value="pageChanged"
    ></v-pagination>
    <v-dialog v-model="dialog" max-width="600px">
        <v-card>
            <v-card-title>
                <span class="headline">{{ t('restoredialog') }}</span>
            </v-card-title>
            <v-card-text>
                <v-form
                    ref="form"
                    v-model="valid"
                >
                    <v-row>
                        <v-col cols="12">
                            <span>Are you sure to restore data from backed up file '{{ backupFileName }}'?</span>
                            <v-text-field
                                ref="input"
                                v-model="fileName"
                                :rules="fileNameRules"
                                label="Backed up file name*"
                                required
                            ></v-text-field>
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDialog">Close</v-btn>
                <v-btn :disabled="fileName !== backupFileName" color="warning" @click="restore">Restore</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onBeforeMount } from 'vue'

import adlsApi from '@/api/resources/adls'
import appStore from '@/store/app.js'
import { useI18n } from 'vue-i18n'

const app = appStore()
const { t } = useI18n()

let logItemFilter = ref("");

const adlsStatus = reactive({ disable: 1, enable: 0 });
const headers = reactive([
                            { text: t("name"), value: "adlsName", sort: false },
                            { text: t("url"), value: "adlsUrl", sort: false },
                            { text: t(""), value: "action", sort: false }
                        ]);
const data = reactive({
    items: [],
    filterItems: [],
    selectedItems: [],
    displayItems: []
});
const state = reactive({
                isEmptyData: false
            });
const page = reactive({
                        current: 1,
                        size : 10,
                        length: 0
                    });
let dialog = ref(false);
let valid = ref(true);
let fileName = ref("");
const fileNameRules = reactive([
                                    v => !!v || "File name is required"
                                ]);
let isMatched = ref(false);
const backupFileName = reactive("zhtimesheet-local-testing-backup.json");


async function loadData() {
    await app.load(async () => {
        const resData = await adlsApi.getAll();
        data.items = resData;
        data.displayItems = resData;
        data.filterItems = resData;
        page.length = parseInt(data.filterItems.length/page.size);
    })
};
async function remove() {
    const ids = data.selectedItems.map(item => item.logTimeId);
    await app.load(async () => {
        if (ids.length > 0)
        {
            await adlsApi.delete(ids);
            data.selectedItems = [];
            data.filterItems = data.filterItems.filter(logwork => !ids.includes(logwork.logTimeId));
        }
    })
    
    app.success(t("deletesuccess"), 5000);

    await loadData()
};
async function restore() {
    valid = $refs.form.validate();
    if(!valid)
        return;
        
        await app.load(async () => {
        await adlsApi.restore();
        closeDialog();
    })

    app.success(t("restoresuccess"), 5000);

    await loadData();
};
function closeDialog() {
    $refs.form.reset();
    dialog = false;
};

function search() {
    data.filterItems = _.filter(data.items, (item) => {
        return item.itemName?.includes(data.logItemFilter)
        || item.ownerName?.includes(data.logItemFilter)
        || item.logDate?.includes(data.logItemFilter)
        || item.projName?.includes(data.logItemFilter)
    });
    page.length = parseInt(data.filterItems.length/page.size);
    pageChanged()
};

function pageChanged() {
    if(page.current == page.length)
    {
        data.displayItems =_.slice(data.filterItems, (page.current -1) * page.size, data.filterItems.length);
    }
    else {
        data.displayItems = page.current < page.length ? _.slice(data.filterItems, (page.current -1) * page.size, page.current * page.size) : data.filterItems;
    }
}
// bind data
onBeforeMount(async () => {
  await loadData()
  pageChanged()
})

</script>