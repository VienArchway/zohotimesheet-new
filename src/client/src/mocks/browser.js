import { setupWorker, rest } from "msw"

export const worker = setupWorker()
const stop = worker.stop()

window.msw = {
    worker,
    rest,
    stop
}
