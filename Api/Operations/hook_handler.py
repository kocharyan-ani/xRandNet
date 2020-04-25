from flask import Flask
from flask import request
import os
import subprocess

app = Flask(__name__)


def run_script(script_command, running_directory=None, env=None):
    current_env = os.environ.copy()
    if env is not None:
        for env_var_name, env_var_value in env.items():
            current_env[env_var_name] = env_var_value
    print(f"Running command '{script_command}'")
    process_pipe = subprocess.Popen(script_command,
                                    shell=True,
                                    executable='/bin/bash',
                                    cwd=running_directory,
                                    stdout=subprocess.PIPE,
                                    stderr=subprocess.PIPE,
                                    env=current_env)
    for line in process_pipe.stdout:
        print(line.decode().strip())
    for line in process_pipe.stderr:
        print(line.decode().strip())
    process_pipe.communicate()

    return process_pipe.returncode


def needToRedeploy(request):
    redeployments = []
    if 'commits' in request:
        for commit in request['commits']:
            if '#PROD' in str(commit['message']):
                return ["Api", "Ui", "Service", "Nginx", "Hook"]
            if 'modified' in commit:
                for modified_file in commit['modified']:
                    if modified_file == 'Api/Operations/xrandnet.service':
                        redeployments.append("Service")
                    elif modified_file == 'Api/Operations/nginx.conf':
                        redeployments.append("Nginx")
                    elif modified_file == 'Api/Operations/hook_handler.py':
                        redeployments.append("Hook")
                    elif str(modified_file).startswith("Api"):
                        redeployments.append("Api")
                    elif str(modified_file).startswith("Ui"):
                        redeployments.append("Ui")
            if 'removed' in commit:
                for removed_file in commit['removed']:
                    if str(removed_file).startswith("Api"):
                        redeployments.append("Api")
                    if str(removed_file).startswith("Ui"):
                        redeployments.append("Ui")
            if 'added' in commit:
                for added_file in commit['added']:
                    if str(added_file).startswith("Api"):
                        redeployments.append("Api")
                    if str(added_file).startswith("Ui"):
                        redeployments.append("Ui")
    return redeployments


@app.route('/github-hook', methods=['POST'])
def handle_hook():
    redeployments = needToRedeploy(request.json)
    print(redeployments)
    if len(redeployments) == 0:
        return "NOT_NEEDED"
    if run_script("/opt/xRandNet/automation/update_codebase.sh") != 0:
        return "FAILURE"
    if run_script("cp /opt/xRandNet/code/Api/Operations/deploy_ui.sh "
                  "/opt/xRandNet/automation/deploy_ui.sh") != 0:
        return "FAILURE"
    if run_script("cp /opt/xRandNet/code/Api/Operations/deploy_api.sh "
                  "/opt/xRandNet/automation/deploy_api.sh") != 0:
        return "FAILURE"
    if run_script("cp /opt/xRandNet/code/Api/Operations/update_codebase.sh "
                  "/opt/xRandNet/automation/update_codebase.sh") != 0:
        return "FAILURE"
    if "Api" in redeployments:
        if run_script("/opt/xRandNet/automation/deploy_api.sh") != 0:
            return "FAILURE"
    if "Ui" in redeployments:
        if run_script("/opt/xRandNet/automation/deploy_ui.sh") != 0:
            return "FAILURE"
    if "Service" in redeployments:
        if run_script("cp /opt/xRandNet/code/Api/Operations/xrandnet.service "
                      "/etc/systemd/system/xrandnet.service") != 0:
            return "FAILURE"
        if run_script("systemctl daemon-reload") != 0:
            return "FAILURE"
        if run_script("service xrandnet restart") != 0:
            return "FAILURE"
    if "Nginx" in redeployments:
        if run_script("cp /opt/xRandNet/code/Api/Operations/nginx.conf "
                      "/etc/nginx/sites-enabled/xrandnet") != 0:
            return "FAILURE"
        if run_script("service nginx restart") != 0:
            return "FAILURE"
    if "Hook" in redeployments:
        if run_script("cp /opt/xRandNet/code/Api/Operations/hook_handler.py "
                      "/opt/xRandNet/automation/hook_handler.py") != 0:
            return "FAILURE"
    return "SUCCESS"


if __name__ == '__main__':
    app.run(debug=True, port=3003)
