<template>
  <div>
    <v-table
        fixed-header
        height="400px" >
        <thead>
            <tr>
                <th class="text-left">
                    {{ t("name") }}
                </th>
                <th class="text-left">
                    {{ t("url") }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr
                v-for="item in items"
                :key="item.webHookId"
            >
                <td>{{ item.webHookName }}</td>
                <td>{{ item.webHookUrl }}</td>
                <td>
                    <v-btn
                        @click="updateStatus(item)"
                        color="primary"
                        dark
                    >
                        {{ t('enable') }}
                    </v-btn>
                </td>
            </tr>
        </tbody>
    </v-table>
  </div>
</template>


<script>
import webhookApi from '@/api/resources/webhook'
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
            logItemFilter: "",
            webhookStatus: {
                disable: 1,
                enable: 0
            },
            headers: [
                { text: this.t("name"), value: "webhookName", sort: false },
                { text: this.t("url"), value: "webhookUrl", sort: false },
                { text: this.t(""), value: "action", sort: false }
            ],
            items: [],
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
            backupFileName: window.backupFileName
        };
    },
    async created() {
        try {
            await app.load(async () => {
              this.items = await webhookApi.getAll();
            })
        }
        catch (error) {
            const resMessage = error.response?.data?.message;
            const errorDetail = JSON.parse(resMessage);
            if (errorDetail) {
                app.error(errorDetail.message, 100000);
            }
            
        }
    },
    methods: {
        async updateStatus(item) {
            try {
                await app.load(async () => {
                  this.items = await webhookApi.update({ WebHookId: item.webHookId, HookStatus: this.webhookStatus.enable}); 
                })
            } catch (error) {
                const resMessage = error.response?.data?.message;
                const errorDetail = JSON.parse(resMessage);
                if (errorDetail) {
                    app.error(errorDetail.message, 100000);
                }
            }

        },
        closeDialog() {
            this.$refs.form.reset();
            this.dialog = false;
        }
    }
})
</script>
