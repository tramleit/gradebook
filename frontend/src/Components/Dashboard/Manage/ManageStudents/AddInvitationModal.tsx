import React, { ReactElement, useEffect, useState } from "react";
import { connect } from "react-redux";
import { useTranslation } from "react-i18next";
import { Modal } from "react-bootstrap";
import {
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Stack,
} from "@mui/material";
import StudentResponse from "../../../../ApiClient/Students/Definitions/StudentResponse";
import StudentsProxy from "../../../../ApiClient/Students/StudentsProxy";
import Person from "../../../Shared/Person";
import PersonSmall from "../../../Shared/PersonSmall";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes } from "@fortawesome/free-solid-svg-icons";
import InvitationsProxy from "../../../../ApiClient/Invitations/InvitationsProxy";
import SchoolRolesEnum from "../../../../Common/Enums/SchoolRolesEnum";
const mapStateToProps = (state: any) => ({});
const mapDispatchToProps = (dispatch: any) => ({});
interface AddInvitationModalProps {
  show: boolean;
  onHide: () => void;
}
const AddInvitationModal = (props: AddInvitationModalProps): ReactElement => {
  const { t } = useTranslation("addInvitationModal");
  const [inactiveStudents, setInactiveStudents] = useState(
    [] as StudentResponse[]
  );
  const [selectedStudents, setSelectedStudents] = useState<string[]>([]);

  useEffect(() => {
    StudentsProxy.getInactiveAccessibleStudents().then((response) => {
      setInactiveStudents(response.data);
    });
  }, []);
  const sendButtonHandler = () => {
    InvitationsProxy.inviteMultiplePeople({
      invitedPersonGuidArray: selectedStudents,
      role: SchoolRolesEnum.Student,
    }).then((response) => {
      props.onHide();
    });
  };
  return (
    <Modal show={props.show} onHide={props.onHide}>
      <Modal.Header closeButton>
        <Modal.Title>{t("addInvitation")}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Stack>
          <Stack>
            <FormControl>
              <InputLabel>{t("selectPeopleToInvite")}</InputLabel>
              <Select
                multiple
                value={selectedStudents}
                onChange={(event) => {
                  const {
                    target: { value },
                  } = event;
                  setSelectedStudents(
                    typeof value === "string" ? value.split(",") : value
                  );
                }}
                renderValue={(selected) => selected.length}
                label={t("selectPeopleToInvite")}
              >
                {inactiveStudents.map((student) => (
                  <MenuItem
                    key={student.guid}
                    value={student.guid}
                    className="row"
                  >
                    <Person
                      name={student.name}
                      surname={student.surname}
                      birthday={student.birthday}
                    />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Stack>
          <Stack>
            <div className="d-flex flex-wrap gap-1 justify-content-center mt-2">
              <>
                {inactiveStudents
                  .filter((student) => selectedStudents.includes(student.guid))
                  .map((student) => (
                    <div className="border rounded-3 p-0" key={student.guid}>
                      <PersonSmall
                        name={student.name}
                        surname={student.surname}
                      />
                      <div className="d-inline mx-2">
                        <FontAwesomeIcon
                          icon={faTimes}
                          className="my-auto text-danger cursor-pointer"
                          onClick={() =>
                            setSelectedStudents(
                              selectedStudents.filter(
                                (guid) => guid != student.guid
                              )
                            )
                          }
                        />
                      </div>
                    </div>
                  ))}
              </>
            </div>
          </Stack>
        </Stack>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="outlined" onClick={sendButtonHandler}>
          {t("addInvitation")}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
export default connect(mapStateToProps, mapDispatchToProps)(AddInvitationModal);
