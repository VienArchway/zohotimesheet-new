<script setup>
import {onMounted, ref} from 'vue'
import {useI18n} from 'vue-i18n'
const { t } = useI18n()
</script>

<template>
  <div>
    <v-row>
        <v-col cols="9">
            <v-text-field
                v-model="logItemFilter"
                append-icon="mdi-magnify"
                :label="t('searchcondition')"
                @change="search"
                hide-details
            ></v-text-field>
        </v-col>
        <v-col>
            <v-btn
                :disabled="!selectedItems.length"
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
        height="400px" >
        <thead>
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
                v-for="item in filterItems"
                :key="item.logTimeId"
            >
                <v-checkbox
                    v-model="selectedItems"
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

<route lang="yaml">
meta:
  layout: default
</route>

<script>
import "./index.scss";
import adlsApi from '@/api/resources/adls'
import appStore from '@/store/app.js'
const app = appStore()

export default {
    components: {
    },
    data() {
        return {
            // currentLocale: this.$i18n.locale,
            logItemFilter: "",
            adlsStatus: {
                disable: 1,
                enable: 0
            },
            headers: [
                { text: this.t("name"), value: "adlsName", sort: false },
                { text: this.t("url"), value: "adlsUrl", sort: false },
                { text: this.t(""), value: "action", sort: false }
            ],
            items: [],
            filterItems: [],
            selectedItems: [],
            state: {
                isEmptyData: false
            },
            dialog: false,
            valid: true,
            fileName: "",
            fileNameRules: [
                v => !!v || "File name is required"
            ],
            isMatched: false,
            backupFileName: "zhtimesheet-local-testing-backup.json" // window.backupFileName
        };
    },
    async created() {
        await this.loadData()
    },
    methods: {
        async loadData() {
            await app.load(async () => {
                const resData = await adlsApi.getAll();
                this.items = resData;
                this.filterItems = resData;
            })
        },
        async remove() {
            const ids = this.selectedItems.map(item => item.logTimeId);
            await app.load(async () => {
                if (ids.length > 0)
                {
                    await adlsApi.delete(ids);
                    this.selectedItems = [];
                    this.items = this.items.filter(logwork => !ids.includes(logwork.logTimeId));
                }
            })
            
            app.success(this.t("deletesuccess"), 5000);

            await this.loadData()
        },
        async restore() {
            this.valid = this.$refs.form.validate();
            if(!this.valid)
                return;
                
             await app.load(async () => {
                await adlsApi.restore();
                this.closeDialog();
            })

            app.success(this.t("restoresuccess"), 5000);

            await this.loadData();
        },
        closeDialog() {
            this.$refs.form.reset();
            this.dialog = false;
        },
        search() {
            this.filterItems = _.filter(this.items, (item) => { return item.itemName?.includes(this.logItemFilter) || item.ownerName?.includes(this.logItemFilter)|| item.logDate?.includes(this.logItemFilter)|| item.projName?.includes(this.logItemFilter)});
        }
    }
};
</script>
