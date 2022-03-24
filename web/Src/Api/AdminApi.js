import { post } from './AxiosInstance.js'

export const getAdminsByAuthority = ({ authority }) => post('/Admin/GetAdminsByAuthority',  { authority })
