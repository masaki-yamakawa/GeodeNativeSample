package geode.nativesample;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;

import org.apache.geode.DataSerializable;
import org.apache.geode.cache.Declarable;

public class Response implements DataSerializable, Declarable {

	private long id;
	private String resValue;

	public Response(long id, String resValue) {
		this.id = id;
		this.resValue = resValue;
	}

	@Override
	public void toData(DataOutput out) throws IOException {
		out.writeLong(id);
		out.writeUTF(resValue);
	}

	@Override
	public void fromData(DataInput in) throws IOException, ClassNotFoundException {
		this.id = in.readLong();
		this.resValue = in.readUTF();
	}

	@Override
	public String toString() {
		return "Response [id=" + id + ", resValue=" + resValue + "]";
	}
}
