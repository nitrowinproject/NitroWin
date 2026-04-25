<div align="center">
<img src="https://raw.githubusercontent.com/nitrowinproject/Branding/main/Assets/Images/NitroWin.png" alt="NitroWin logo" width="800">

---

![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/nitrowinproject/NitroWin/total?logo=github&logoColor=bcafb6&label=Downloads&labelColor=2d222e&color=7d7688)
![GitHub License](https://img.shields.io/github/license/nitrowinproject/NitroWin?logo=github&logoColor=bcafb6&label=License&labelColor=2d222e&color=7d7688)
![GitHub Release](https://img.shields.io/github/v/release/nitrowinproject/NitroWin?logo=github&logoColor=bcafb6&label=Release&labelColor=2d222e&color=7d7688)

A clean, debloated, optimized and privacy-friendly Windows modification.

</div>

## :sparkles: Features

Feature | NitroWin | Stock Windows 11 | Other Windows modifications
------- | -------- | ---------------- | ---------------------------
:lock: Better Privacy | :white_check_mark: | :no_entry_sign: | :white_check_mark:
:scissors: Less bloat | :white_check_mark: | :no_entry_sign: | :white_check_mark:
:rocket: Better performance | :white_check_mark: | :no_entry_sign: | :white_check_mark:
:shield: No disabled security features | :white_check_mark: | :white_check_mark: | :no_entry_sign:
:do_not_litter: No intrusive custom branding | :white_check_mark: | :white_check_mark: | :no_entry_sign:
:no_entry_sign: No AME Playbook | :white_check_mark: | :white_check_mark: | :no_entry_sign:

## :books: Usage

> [!CAUTION]
> Either a Pro or an Enterprise version of Windows 11 is required, as other versions do not support Group Policies.

> [!TIP]
> Check out the [documentation](docs) if you have any questions.

1. Download the [latest version of NitroWin](https://github.com/nitrowinproject/NitroWin/releases/latest).

2. Extract it to the root of your installation media. Your file structure should now look like this:

```text
D: (or whatever your installation media is)/
├── (Windows setup files)
├── autounattend.xml
└── NitroWin/
    ├── NitroWin.exe
    └── Configuration/
        ├── (optional, not included by default) Apps.yml
        └── (optional, not included by default) Configuration.yml
```

3. Install Windows as usual.

4. (Optional, but highly recommended) Use [WinUtil](https://github.com/ChrisTitusTech/winutil) and [OOSU](https://www.oo-software.com/en/shutup10) for even better privacy.

## :scroll: License

This project is licensed under the [Zero-Clause BSD License](LICENSE).

NitroWin also uses parts of other projects which are licensed under their respective licenses. See [this document](docs/other-projects.md) for more details.

### :heavy_exclamation_mark: Disclaimer

This project is not affiliated with Microsoft or any other third-party projects referenced in this repository.

This project does not distribute modified Windows ISOs.
