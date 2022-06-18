<script setup>
import {onMounted, ref} from 'vue'
import {useI18n} from 'vue-i18n'
import { getVerifyTokenApi } from '@/api/resources/zohoToken'

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
             try {
                const resData = await adlsApi.getAll();
                this.items = resData;
                this.filterItems = resData;
            }
            catch (error) {
                const resMessage = error.response?.data?.message;
                const errorDetail = JSON.parse(resMessage);
                if (errorDetail) {
                    this.$store.commit("notify.error", { content: errorDetail.message, timeout:100000 });
                }
                
            } finally {
                // this.$store.commit("closeLoading");
            }
        },
        async remove() {
            const ids = ["52523000002003313"] //this.selectedItems.map(item => item.logTimeId);
            
            try {
                if (ids.length > 0)
                {
                    // this.$store.commit("showLoading");

                    await adlsApi.delete(ids);
                    this.selectedItems = [];
                    this.items = this.items.filter(logwork => !ids.includes(logwork.logTimeId));

                    // this.$store.commit("notify.success", { content: this.t("deletesuccess"), timeout:3000 });

                    // this.$store.commit("closeLoading");
                }
            }
            catch (error) {
                const resMessage = error.response?.data?.message;
                const errorDetail = JSON.parse(resMessage);
                if (errorDetail) {
                    this.$store.commit("notify.error", { content: errorDetail.message, timeout:100000 });
                }
                
            } finally {
                // this.$store.commit("closeLoading");
            }
        },
        async restore() {
            this.valid = this.$refs.form.validate();
            if(!this.valid)
                return;

            // this.$store.commit("showLoading");


            const response = await adlsApi.restore();
            if(response.status !== 200) {
                // this.$store.commit("closeLoading");
                return;
            }
            // this.$store.commit("notify.success", { content: this.t("restoresuccess"), timeout:3000 });
            this.closeDialog();
            this.loadData();
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
